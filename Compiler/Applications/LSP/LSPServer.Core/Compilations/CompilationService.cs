using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Core.Extensions;
using KSPCompiler.Commons;
using KSPCompiler.Controllers.Compiler;
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

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace KSPCompiler.Applications.LSPServer.Core.Compilations;

public sealed class CompilationService
{
    public sealed class ExecuteCompileOption(
        bool enableObfuscation = false
    )
    {
        public bool EnableObfuscation { get; } = enableObfuscation;
    }

    private readonly AggregateSymbolTable builtInSymbolTable = new(
        builtInVariables: new VariableSymbolTable(),
        userVariables: new VariableSymbolTable(),
        uiTypes: new UITypeSymbolTable(),
        commands: new CommandSymbolTable(),
        builtInCallbacks: new CallbackSymbolTable(),
        userCallbacks: new CallbackSymbolTable(),
        userFunctions: new UserFunctionSymbolTable(),
        preProcessorSymbols: new PreProcessorSymbolTable()
    );

    private ILanguageServerFacade ServerFacade { get; }
    private ILanguageServerConfiguration Configuration { get; }

    private CompilerCacheService CompilerCacheService { get; }
    private IEventEmitter CompilerEventEmitter { get; } = new EventEmitter();

    private readonly CompilerController compilerController = new();

    public CompilationService(
        ILanguageServerFacade serverFacade,
        ILanguageServerConfiguration configuration,
        CompilerCacheService compilerCacheService )
    {
        ServerFacade  = serverFacade;
        Configuration = configuration;
        CompilerCacheService = compilerCacheService;

        // 外部定義ファイルからビルトイン変数、コマンド、コールバック、UIタイプを構築
        LoadSymbolTables( builtInSymbolTable );
    }

    #region Compilation
    private async Task<CompilerResult> CompileAsync( string script, IEventEmitter eventEmitter, ExecuteCompileOption executeOption, CancellationToken cancellationToken )
    {
        SetupSymbol( builtInSymbolTable );

        var parser = new AntlrKspStringSyntaxParser( script, eventEmitter, Encoding.UTF8 );
        var symbolTableInScript = builtInSymbolTable.CreateBuiltInSymbolsOnly();

        var option = new CompilerOption(
            SyntaxParser: parser,
            SymbolTable: symbolTableInScript,
            EnableObfuscation: executeOption.EnableObfuscation
        );

        return await compilerController.ExecuteAsync( eventEmitter, option, cancellationToken );
    }

    public async Task<CompilerResult> ExecuteCompilationAsync( DocumentUri uri, ExecuteCompileOption executeOption, CancellationToken cancellationToken )
    {
        var script = await File.ReadAllTextAsync( uri.Path, cancellationToken );

        await ClearDiagnosticAsync( uri );

        return await ExecuteCompilationAsync( uri, script, executeOption, cancellationToken );
    }

    public async Task<CompilerResult> ExecuteCompilationAsync( DocumentUri uri, string script, ExecuteCompileOption executeOption, CancellationToken cancellationToken )
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
        var result = await CompileAsync( script, CompilerEventEmitter, executeOption, cancellationToken );

        // エラー、警告を送信
        ServerFacade.TextDocument.PublishDiagnostics( new PublishDiagnosticsParams
            {
                Uri         = uri,
                Diagnostics = diagnostics.ToImmutable()
            }
        );

        var allLinesText = GetScriptLines( script );

        CompilerCacheService.UpdateCache(
            uri,
            new CompilerCacheItem( uri, allLinesText, result.SymbolTable, result.Ast )
        );

        return result;
    }
    #endregion

    #region Handling Events
    public async Task HandleOpenTextDocumentAsync( DidOpenTextDocumentParams request, CancellationToken cancellationToken )
    {
        var uri = request.TextDocument.Uri;
        var script = request.TextDocument.Text;

        var executeOption = new ExecuteCompileOption(
            enableObfuscation: false
        );

        await ExecuteCompilationAsync( uri, script, executeOption, cancellationToken );
    }

    public async Task HandleCloseTextDocumentAsync( DidCloseTextDocumentParams request, CancellationToken cancellationToken )
    {
        await ClearDiagnosticAsync( request.TextDocument.Uri );
    }


    public async Task HandleChangTextDocumentAsync( DidChangeTextDocumentParams request, CancellationToken cancellationToken )
    {
        var uri = request.TextDocument.Uri;
        var script = request.ContentChanges.First().Text;

        var executeOption = new ExecuteCompileOption(
            enableObfuscation: false
        );

        await ClearDiagnosticAsync( uri );
        await ExecuteCompilationAsync( uri, script, executeOption, cancellationToken );
    }

    public async Task ClearDiagnosticAsync( DocumentUri uri )
    {
        ServerFacade.TextDocument.PublishDiagnostics( new PublishDiagnosticsParams
            {
                Uri         = uri,
                Diagnostics = ImmutableArray<Diagnostic>.Empty
            }
        );

        await Task.CompletedTask;
    }
    #endregion

    #region Setup Symbols
    private static void LoadSymbolTables( AggregateSymbolTable symbolTable )
    {
        var baseDir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) ?? ".";

        var basePath = Path.Combine( baseDir, "Data", "Symbols" );

        using ISymbolRepository<VariableSymbol> variables = new VariableSymbolRepository( Path.Combine( basePath, "variables.yaml" ) );
        symbolTable.BuiltInVariables.AddRange( variables.FindAll() );

        using ISymbolRepository<UITypeSymbol> uiTypes = new UITypeSymbolRepository( Path.Combine( basePath, "uitypes.yaml" ) );
        symbolTable.UITypes.AddRange( uiTypes.FindAll() );

        using ISymbolRepository<CommandSymbol> commands = new CommandSymbolRepository( Path.Combine( basePath, "commands.yaml" ) );
        symbolTable.Commands.AddRange( commands.FindAll() );

        using ISymbolRepository<CallbackSymbol> callbacks = new CallbackSymbolRepository( Path.Combine( basePath, "callbacks.yaml" ) );
        var foundCallbacks = callbacks.FindAll();

        foreach( var x in foundCallbacks )
        {
            if( x.AllowMultipleDeclaration )
            {
                symbolTable.BuiltInCallbacks.AddAsOverload( x, x.Arguments );
            }
            else
            {
                symbolTable.BuiltInCallbacks.AddAsNoOverload( x );
            }
        }
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
    #endregion

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
    #endregion
}
