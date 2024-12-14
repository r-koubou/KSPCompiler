using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
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
    private IEventEmitter CompilerEventEmitter { get; } = new EventEmitter();

    private TextDocumentSelector TextDocumentSelector { get; } = new(
        new TextDocumentFilter
        {
            Pattern = "**/*.txt"
        },
        new TextDocumentFilter
        {
            Pattern = "**/*.ksp"
        }
    );

    public TextDocumentHandler(
        ILanguageServerFacade serverFacade,
        ILanguageServerConfiguration configuration,
        CompilerService compilerService )
    {
        ServerFacade    = serverFacade;
        Configuration   = configuration;
        CompilerService = compilerService;
    }

    private TextDocumentSyncKind Change
        => TextDocumentSyncKind.Full;


    public override TextDocumentAttributes GetTextDocumentAttributes( DocumentUri uri )
    {
        return new TextDocumentAttributes( uri, "ksp" );
    }

    public override async Task<Unit> Handle( DidOpenTextDocumentParams request, CancellationToken cancellationToken )
    {
        await Task.CompletedTask;
        return Unit.Value;
    }

    public override async Task<Unit> Handle( DidChangeTextDocumentParams request, CancellationToken cancellationToken )
    {
        var diagnostics = ImmutableArray<Diagnostic>.Empty.ToBuilder();
        var script = request.ContentChanges.First().Text;

        using var compilerEventSubscribers = new CompositeDisposable();
        CompilerEventEmitter.Subscribe<CompilationErrorEvent>(
            e => diagnostics.Add( CompilationEventExtension.AsDiagnostic( (CompilationErrorEvent)e ) )
        ).AddTo( compilerEventSubscribers );
        CompilerEventEmitter.Subscribe<CompilationWarningEvent>( e => diagnostics.Add( e.AsDiagnostic() ) ).AddTo( compilerEventSubscribers );

        CompilerService.Compile( script, CompilerEventEmitter );

        if( diagnostics.Count != 0 )
        {
            ServerFacade.TextDocument.PublishDiagnostics( new PublishDiagnosticsParams
                {
                    Uri         = request.TextDocument.Uri,
                    Diagnostics = diagnostics.ToImmutable()
                }
            );
        }

        await Task.CompletedTask;
        return Unit.Value;
    }

    public override async Task<Unit> Handle( DidSaveTextDocumentParams request, CancellationToken cancellationToken )
    {
        await Task.CompletedTask;
        return Unit.Value;
    }

    public override async Task<Unit> Handle( DidCloseTextDocumentParams request, CancellationToken cancellationToken )
    {
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
}
