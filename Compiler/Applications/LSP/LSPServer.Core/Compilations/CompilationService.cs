using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Controllers.Compiler;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables;
using KSPCompiler.Infrastructures.Parser.Antlr;
using KSPCompiler.LSPServer.Core.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace KSPCompiler.LSPServer.Core.Compilations;

public class CompilationService
{
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

    private CompilerCache CompilerCache { get; }
    private IEventEmitter CompilerEventEmitter { get; } = new EventEmitter();

    private readonly CompilerController compilerController = new();

    public CompilationService(
        ILanguageServerFacade serverFacade,
        ILanguageServerConfiguration configuration,
        CompilerCache compilerCache )
    {
        ServerFacade  = serverFacade;
        Configuration = configuration;
        CompilerCache = compilerCache;

        // 外部定義ファイルからビルトイン変数、コマンド、コールバック、UIタイプを構築
        LoadSymbolTables( builtInSymbolTable );
    }

    #region Compilation
    private async Task<CompilerResult> CompileAsync( string script, IEventEmitter eventEmitter, CancellationToken cancellationToken )
    {
        SetupSymbol( builtInSymbolTable );

        var parser = new AntlrKspStringSyntaxParser( script, eventEmitter, Encoding.UTF8 );
        var symbolTableInScript = builtInSymbolTable.CreateBuiltInSymbolsOnly();

        var option = new CompilerOption(
            SyntaxParser: parser,
            SymbolTable: symbolTableInScript,
            EnableObfuscation: false
        );

        return await compilerController.ExecuteAsync( eventEmitter, option, cancellationToken );
    }

    private async Task<CompilerResult> ExecuteCompilationAsync( DocumentUri uri, string script, CancellationToken cancellationToken )
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
        var result = await CompileAsync( script, CompilerEventEmitter, cancellationToken );

        // エラー、警告を送信
        ServerFacade.TextDocument.PublishDiagnostics( new PublishDiagnosticsParams
            {
                Uri         = uri,
                Diagnostics = diagnostics.ToImmutable()
            }
        );

        var allLinesText = GetScriptLines( script );

        CompilerCache.UpdateCache(
            uri,
            new CompilerCacheItem( allLinesText, result.SymbolTable, result.Ast )
        );

        return result;
    }
    #endregion

    #region Handling Events
    public async Task HandleOpenTextDocumentAsync( DidOpenTextDocumentParams request, CancellationToken cancellationToken )
    {
        var uri = request.TextDocument.Uri;
        var script = request.TextDocument.Text;

        await ExecuteCompilationAsync( uri, script, cancellationToken );
    }

    public async Task HandleCloseTextDocumentAsync( DidCloseTextDocumentParams request, CancellationToken cancellationToken )
    {
        await ClearDiagnosticAsync( request.TextDocument.Uri );
    }


    public async Task HandleChangTextDocumentAsync( DidChangeTextDocumentParams request, CancellationToken cancellationToken )
    {
        var uri = request.TextDocument.Uri;
        var script = request.ContentChanges.First().Text;

        await ClearDiagnosticAsync( uri );
        await ExecuteCompilationAsync( uri, script, cancellationToken );
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
    #endregion

    #region Setup Symbols
    private static void LoadSymbolTables( AggregateSymbolTable symbolTable )
    {
        var basePath = Path.Combine( "Data", "Symbols" );

        using ISymbolRepository<VariableSymbol> variables = new VariableSymbolRepository( Path.Combine( basePath, "variables.json" ) );
        symbolTable.BuiltInVariables.AddRange( variables.FindAll() );

        using ISymbolRepository<UITypeSymbol> uiTypes = new UITypeSymbolRepository( Path.Combine( basePath, "uitypes.json" ) );
        symbolTable.UITypes.AddRange( uiTypes.FindAll() );

        using ISymbolRepository<CommandSymbol> commands = new CommandSymbolRepository( Path.Combine( basePath, "commands.json" ) );
        symbolTable.Commands.AddRange( commands.FindAll() );

        using ISymbolRepository<CallbackSymbol> callbacks = new CallbackSymbolRepository( Path.Combine( basePath, "callbacks.json" ) );
        symbolTable.BuiltInCallbacks.AddRange( callbacks.FindAll() );
    }

    private static void SetupSymbol( AggregateSymbolTable symbolTable )
    {
        // 再突入時のため、ユーザー定義シンボルをクリア
        symbolTable.UserVariables.RemoveNoReservedSymbols();
        symbolTable.UserCallbacks.RemoveNoReservedSymbols();
        symbolTable.UserFunctions.RemoveNoReservedSymbols();

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
