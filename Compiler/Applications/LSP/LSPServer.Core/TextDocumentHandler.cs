using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Controllers.Compiler;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.LSPServer.Core.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;

using Unit = MediatR.Unit;

namespace KSPCompiler.LSPServer.Core;

internal class TextDocumentHandler : TextDocumentSyncHandlerBase
{
    private ILanguageServerFacade ServerFacade { get; }
    private ILanguageServerConfiguration Configuration { get; }

    private CompilerService CompilerService { get; }
    private CompilerCache CompilerCache { get; }
    private IEventEmitter CompilerEventEmitter { get; } = new EventEmitter();

    private TextDocumentSelector TextDocumentSelector { get; } = new(
        new TextDocumentFilter
        {
            Language = "ksp"
        }
    );

    public TextDocumentHandler(
        ILanguageServerFacade serverFacade,
        ILanguageServerConfiguration configuration,
        CompilerService compilerService,
        CompilerCache compilerCache )
    {
        ServerFacade    = serverFacade;
        Configuration   = configuration;
        CompilerService = compilerService;
        CompilerCache   = compilerCache;
    }

    private TextDocumentSyncKind Change
        => TextDocumentSyncKind.Full;


    public override TextDocumentAttributes GetTextDocumentAttributes( DocumentUri uri )
    {
        return new TextDocumentAttributes( uri, "ksp" );
    }

    public override async Task<Unit> Handle( DidOpenTextDocumentParams request, CancellationToken cancellationToken )
    {
        var uri = request.TextDocument.Uri;
        var script = request.TextDocument.Text;

        _ = await ExecuteCompilationAsync( uri, script, cancellationToken );

        return Unit.Value;
    }

    public override async Task<Unit> Handle( DidChangeTextDocumentParams request, CancellationToken cancellationToken )
    {
        var uri = request.TextDocument.Uri;
        var script = request.ContentChanges.First().Text;

        _ = await ExecuteCompilationAsync( uri, script, cancellationToken );

        return Unit.Value;
    }

    public override async Task<Unit> Handle( DidSaveTextDocumentParams request, CancellationToken cancellationToken )
    {
        await Task.CompletedTask;
        return Unit.Value;
    }

    public override async Task<Unit> Handle( DidCloseTextDocumentParams request, CancellationToken cancellationToken )
    {
        CompilerCache.RemoveSymbolTable( request.TextDocument.Uri );

        await Task.CompletedTask;
        return Unit.Value;
    }

    protected override TextDocumentSyncRegistrationOptions CreateRegistrationOptions( TextSynchronizationCapability capability, ClientCapabilities clientCapabilities )
    {
        return new TextDocumentSyncRegistrationOptions
        {
            DocumentSelector = TextDocumentSelector,
            Change           = Change,
            Save = new SaveOptions
            {
                IncludeText = true
            }
        };
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
        var result = await CompilerService.CompileAsync( script, CompilerEventEmitter, cancellationToken );

        // エラー、警告を送信
        ServerFacade.TextDocument.PublishDiagnostics( new PublishDiagnosticsParams
            {
                Uri         = uri,
                Diagnostics = diagnostics.ToImmutable()
            }
        );

        if( result.Result )
        {
            CompilerCache.SetSymbolTable( uri, result.SymbolTable );
        }

        return result;
    }
}
