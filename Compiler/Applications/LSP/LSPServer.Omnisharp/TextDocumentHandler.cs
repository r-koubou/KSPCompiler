using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.CoreNew;
using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.EventEmitting.Extensions;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.Infrastructures.EventEmitting.Default;
using KSPCompiler.Infrastructures.Parser.Antlr;
using KSPCompiler.Interactors.ApplicationServices.Compilation;
using KSPCompiler.LSPServer.Omnisharp.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;

using Unit = MediatR.Unit;

namespace KSPCompiler.LSPServer.Omnisharp;

internal class TextDocumentHandler : TextDocumentSyncHandlerBase
{
    public sealed class ExecuteCompileOption(
        bool enableObfuscation = false
    )
    {
        public bool EnableObfuscation { get; } = enableObfuscation;
    }

    private readonly AggregateSymbolTable builtInSymbolTable;

    private ILanguageServerFacade ServerFacade { get; }
    private ILanguageServerConfiguration Configuration { get; }

    private CompilationApplicationService CompilationService { get; } = new();
    private CompilationCacheManager CompilerCacheService { get; } = new();
    private IEventEmitter CompilerEventEmitter { get; } = new EventEmitter();

    public TextDocumentHandler(
        ILanguageServerFacade serverFacade,
        ILanguageServerConfiguration configuration )
    {
        ServerFacade       = serverFacade;
        Configuration      = configuration;
        builtInSymbolTable = LoadSymbolTables();
    }

    private TextDocumentSyncKind Change
        => TextDocumentSyncKind.Full;

    public override TextDocumentAttributes GetTextDocumentAttributes( DocumentUri uri )
    {
        return new TextDocumentAttributes( uri, ConstantValues.LanguageId );
    }

    public override async Task<Unit> Handle( DidOpenTextDocumentParams request, CancellationToken cancellationToken )
    {
        var script = request.TextDocument.Text;

        await DoCompileAsync( request.TextDocument.Uri, script, cancellationToken );

        return Unit.Value;
    }

    public override async Task<Unit> Handle( DidChangeTextDocumentParams request, CancellationToken cancellationToken )
    {
        var script = request.ContentChanges.First().Text;

        await DoCompileAsync( request.TextDocument.Uri, script, cancellationToken );

        return Unit.Value;
    }

    public override Task<Unit> Handle( DidSaveTextDocumentParams request, CancellationToken cancellationToken )
    {
        return Task.FromResult( Unit.Value );
    }

    public override async Task<Unit> Handle( DidCloseTextDocumentParams request, CancellationToken cancellationToken )
    {
        await ClearDiagnosticAsync( request.TextDocument.Uri );

        return Unit.Value;
    }

    protected override TextDocumentSyncRegistrationOptions CreateRegistrationOptions( TextSynchronizationCapability capability, ClientCapabilities clientCapabilities )
    {
        return new TextDocumentSyncRegistrationOptions
        {
            DocumentSelector = ConstantValues.TextDocumentSelector,
            Change           = Change,
            Save = new SaveOptions
            {
                IncludeText = true
            }
        };
    }

    private async Task ClearDiagnosticAsync( DocumentUri uri )
    {
        ServerFacade.TextDocument.PublishDiagnostics( new PublishDiagnosticsParams
            {
                Uri         = uri,
                Diagnostics = ImmutableArray<Diagnostic>.Empty
            }
        );

        await Task.CompletedTask;
    }

    #region Compile
    private async Task<CompilationResult> ExecuteCompilationAsync( DocumentUri uri, string script, ExecuteCompileOption executeOption, CancellationToken cancellationToken )
    {
        // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
        var diagnostics = ImmutableArray<Diagnostic>.Empty.ToBuilder();

        using var compilerEventSubscribers = new CompositeDisposable();

        // コンパイラ内のエラーを Diagnostics に変換
        CompilerEventEmitter.Subscribe<CompilationErrorEvent>(
            e =>
            {
                diagnostics.Add( e.AsDiagnostic() );
            }
        ).AddTo( compilerEventSubscribers );

        // コンパイラ内の警告を Diagnostics に変換
        CompilerEventEmitter.Subscribe<CompilationWarningEvent>( e =>
            {
                diagnostics.Add( e.AsDiagnostic() );
            }
        ).AddTo( compilerEventSubscribers );


        // コンパイラ実行

        SetupSymbol( builtInSymbolTable );

        var compilationOption = new CompilationOption(
            new AntlrKspStringSyntaxParser( script, CompilerEventEmitter, Encoding.UTF8 ),
            builtInSymbolTable.CreateBuiltInSymbolsOnly(),
            executeOption.EnableObfuscation
            );

        var result = await CompilationService.ExecuteAsync( CompilerEventEmitter, compilationOption, cancellationToken );

        // エラー、警告を送信
        ServerFacade.TextDocument.PublishDiagnostics( new PublishDiagnosticsParams
            {
                Uri         = uri,
                Diagnostics = diagnostics.ToImmutable()
            }
        );

        var allLinesText = GetScriptLines( script );
        var scriptLocation = new ScriptLocation( uri.Path );

        CompilerCacheService.UpdateCache(
            scriptLocation,
            new CompilationCacheItem( scriptLocation, allLinesText, result.SymbolTable, result.Ast )
        );

        return result;
    }

    private async Task DoCompileAsync( DocumentUri uri, string script, CancellationToken cancellationToken )
    {
        await ClearDiagnosticAsync( uri );

        var executeOption = new ExecuteCompileOption(
            enableObfuscation: false
        );

        await ClearDiagnosticAsync( uri );
        await ExecuteCompilationAsync( uri, script, executeOption, cancellationToken );
    }
    #endregion ~Compile

    #region Setup Symbols
    private static AggregateSymbolTable LoadSymbolTables()
    {
        var baseDir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) ?? ".";
        var basePath = Path.Combine( baseDir, "Data", "Symbols" );

        var symbolTables = new AggregateSymbolTable(
            builtInVariables: new VariableSymbolTable(),
            userVariables: new VariableSymbolTable(),
            uiTypes: new UITypeSymbolTable(),
            commands: new CommandSymbolTable(),
            builtInCallbacks: new CallbackSymbolTable(),
            userCallbacks: new CallbackSymbolTable(),
            userFunctions: new UserFunctionSymbolTable(),
            preProcessorSymbols: new PreProcessorSymbolTable()
        );

        using ISymbolRepository<VariableSymbol> variables = new VariableSymbolRepository( Path.Combine( basePath, "variables.yaml" ) );
        symbolTables.BuiltInVariables.AddRange( variables.FindAll() );

        using ISymbolRepository<UITypeSymbol> uiTypes = new UITypeSymbolRepository( Path.Combine( basePath, "uitypes.yaml" ) );
        symbolTables.UITypes.AddRange( uiTypes.FindAll() );

        using ISymbolRepository<CommandSymbol> commands = new CommandSymbolRepository( Path.Combine( basePath, "commands.yaml" ) );
        symbolTables.Commands.AddRange( commands.FindAll() );

        using ISymbolRepository<CallbackSymbol> callbacks = new CallbackSymbolRepository( Path.Combine( basePath, "callbacks.yaml" ) );
        var foundCallbacks = callbacks.FindAll();

        foreach( var x in foundCallbacks )
        {
            if( x.AllowMultipleDeclaration )
            {
                symbolTables.BuiltInCallbacks.AddAsOverload( x, x.Arguments );
            }
            else
            {
                symbolTables.BuiltInCallbacks.AddAsNoOverload( x );
            }
        }

        return symbolTables;
    }

    private static void SetupSymbol( AggregateSymbolTable symbolTable )
    {
        // 再突入時のため、ユーザー定義シンボルをクリア
        symbolTable.UserVariables.Clear();
        symbolTable.UserCallbacks.Clear();
        symbolTable.UserFunctions.Clear();

        // ビルトイン変数は初期化済み扱い
        foreach( var variable in symbolTable.BuiltInVariables )
        {
            variable.State = SymbolState.Initialized;
        }
    }
    #endregion ~Setup Symbols

    #region Common Logic
    private List<string> GetScriptLines( string script )
    {
        var result = new List<string>( 256 );
        using var reader = new StringReader( script );

        while( reader.ReadLine() is {} line )
        {
            result.Add( line );
        }

        return result;
    }
    #endregion ~Common Logic
}
