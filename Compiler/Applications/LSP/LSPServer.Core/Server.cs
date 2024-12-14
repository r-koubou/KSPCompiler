using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.LSPServer.Core.Extensions;

using Microsoft.Extensions.Logging;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Window;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using OmniSharp.Extensions.LanguageServer.Server;

using Unit = MediatR.Unit;

namespace KSPCompiler.LSPServer.Core;

public class Server
{
    public class Option
    {
        public Stream Input { get;  }
        public Stream Output { get; }
        public LogLevel MinimumLevel { get; }

        public Option( Stream input, Stream output, LogLevel minimumLevel )
        {
            Input    = input;
            Output   = output;
            MinimumLevel = minimumLevel;
        }
    }

    public static async Task<LanguageServer> Create( Option option )
    {
        return await LanguageServer.From(
            options => options
                      .WithInput( option.Input )
                      .WithOutput( option.Output )
                      .ConfigureLogging( logging =>
                           {
                               logging.SetMinimumLevel( option.MinimumLevel );
                           }
                       )
                      .WithHandler<TextDocumentHandler>()
                      .WithHandler<DidChangeWatchedFilesHandler>()
                      .OnInitialize( ( server, request, token ) =>
                           {
                               server.LogInfo( "Server initialized." );
                               return Task.CompletedTask;
                           }
                       )
                      .OnStarted( ( server, token ) =>
                           {
                               server.LogInfo( "Server started." );
                               return Task.CompletedTask;
                           }
                       )
        );
    }
}

internal class DidChangeWatchedFilesHandler : IDidChangeWatchedFilesHandler
{
    public DidChangeWatchedFilesRegistrationOptions GetRegistrationOptions() => new();

    public Task<Unit> Handle(DidChangeWatchedFilesParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }

    public DidChangeWatchedFilesRegistrationOptions GetRegistrationOptions(DidChangeWatchedFilesCapability capability, ClientCapabilities clientCapabilities) => new DidChangeWatchedFilesRegistrationOptions();
}


internal class TextDocumentHandler : TextDocumentSyncHandlerBase
{
    private ILanguageServerFacade ServerFacade { get; }
    private ILanguageServerConfiguration Configuration { get; }

    private CompilerService CompilerService { get; } = new();
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

    public TextDocumentHandler( ILanguageServerFacade serverFacade, ILanguageServerConfiguration configuration )
    {
        ServerFacade   = serverFacade;
        Configuration = configuration;
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
            e => diagnostics.Add( e.AsDiagnostic() )
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
