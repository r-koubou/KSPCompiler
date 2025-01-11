using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Events;
using KSPCompiler.LSPServer.Core.Compilations;

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

    private CompilationService CompilationService { get; }
    private CompilerCacheService CompilerCacheService { get; }
    private IEventEmitter CompilerEventEmitter { get; } = new EventEmitter();

    public TextDocumentHandler(
        ILanguageServerFacade serverFacade,
        ILanguageServerConfiguration configuration,
        CompilationService compilationService,
        CompilerCacheService compilerCacheService )
    {
        ServerFacade       = serverFacade;
        Configuration      = configuration;
        CompilationService = compilationService;
        CompilerCacheService      = compilerCacheService;
    }

    private TextDocumentSyncKind Change
        => TextDocumentSyncKind.Full;

    public override TextDocumentAttributes GetTextDocumentAttributes( DocumentUri uri )
    {
        return new TextDocumentAttributes( uri, ConstantValues.LanguageId );
    }

    public override async Task<Unit> Handle( DidOpenTextDocumentParams request, CancellationToken cancellationToken )
    {
        await CompilationService.HandleOpenTextDocumentAsync( request, cancellationToken );

        return Unit.Value;
    }

    public override async Task<Unit> Handle( DidChangeTextDocumentParams request, CancellationToken cancellationToken )
    {
        await CompilationService.HandleChangTextDocumentAsync( request, cancellationToken );

        return Unit.Value;
    }

    public override async Task<Unit> Handle( DidSaveTextDocumentParams request, CancellationToken cancellationToken )
    {
        await Task.CompletedTask;

        return Unit.Value;
    }

    public override async Task<Unit> Handle( DidCloseTextDocumentParams request, CancellationToken cancellationToken )
    {
        await CompilationService.HandleCloseTextDocumentAsync( request, cancellationToken );

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
}
