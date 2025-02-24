using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Omnisharp.Extensions;
using KSPCompiler.Commons;
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
using KSPCompiler.Interactors.ApplicationServices.Symbols;
using KSPCompiler.Interactors.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.Compilations;

public sealed class CompilationService
{
    public sealed class ExecuteCompileOption(
        bool enableObfuscation = false
    )
    {
        public bool EnableObfuscation { get; } = enableObfuscation;
    }

    private ILanguageServerFacade ServerFacade { get; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private ILanguageServerConfiguration Configuration { get; }

    private CompilerCacheService CompilerCacheService { get; }
    private IEventEmitter CompilerEventEmitter { get; } = new EventEmitter();

    private readonly CompilationApplicationService compilationApplicationService;

    public CompilationService(
        ILanguageServerFacade serverFacade,
        ILanguageServerConfiguration configuration,
        CompilerCacheService compilerCacheService )
    {
        ServerFacade  = serverFacade;
        Configuration = configuration;
        CompilerCacheService = compilerCacheService;

        using var repositories = CreateSymbolRepositories();

        compilationApplicationService = new CompilationApplicationService(
            new LoadingBuiltinSymbolApplicationService(
                new LoadBuiltinSymbolInteractor(),
                repositories
            )
        );
    }

    #region Compilation
    private async Task<CompilationResult> CompileAsync( string script, IEventEmitter eventEmitter, ExecuteCompileOption executeOption, CancellationToken cancellationToken )
    {
        var parser = new AntlrKspStringSyntaxParser( script, eventEmitter, Encoding.UTF8 );
        var option = new CompilationOption(
            SyntaxParser: parser,
            EnableObfuscation: executeOption.EnableObfuscation
        );

        return await compilationApplicationService.ExecuteAsync( eventEmitter, option, cancellationToken );
    }

    public async Task<CompilationResult> ExecuteCompilationAsync( DocumentUri uri, ExecuteCompileOption executeOption, CancellationToken cancellationToken )
    {
        var script = await File.ReadAllTextAsync( uri.Path, cancellationToken );

        await ClearDiagnosticAsync( uri );

        return await ExecuteCompilationAsync( uri, script, executeOption, cancellationToken );
    }

    public async Task<CompilationResult> ExecuteCompilationAsync( DocumentUri uri, string script, ExecuteCompileOption executeOption, CancellationToken cancellationToken )
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
    private static AggregateSymbolRepository CreateSymbolRepositories()
    {
        var baseDir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) ?? ".";
        var basePath = Path.Combine( baseDir, "Data", "Symbols" );

        return new AggregateSymbolRepository(
            new VariableSymbolRepository( Path.Combine( basePath, "variables.yaml" ) ),
            new UITypeSymbolRepository( Path.Combine( basePath, "uitypes.yaml" ) ),
            new CommandSymbolRepository( Path.Combine( basePath, "commands.yaml" ) ),
            new CallbackSymbolRepository( Path.Combine( basePath, "callbacks.yaml" )
            )
        );
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
