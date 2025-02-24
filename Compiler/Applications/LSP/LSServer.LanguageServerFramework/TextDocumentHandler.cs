using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server.Options;
using EmmyLua.LanguageServer.Framework.Protocol.Message.TextDocument;
using EmmyLua.LanguageServer.Framework.Protocol.Model.TextEdit;
using EmmyLua.LanguageServer.Framework.Server;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSPServer.Core.Compilation;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Compilation;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.Interactors.ApplicationServices.Compilation;
using KSPCompiler.Interactors.ApplicationServices.Symbols;
using KSPCompiler.Interactors.Symbols;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework;

public sealed class TextDocumentHandler(
    LanguageServer server,
    CompilationCacheManager compilationCacheManager,
    AggregateSymbolRepository symbolRepositories
) : TextDocumentHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;

    private readonly CompilationServerService compilationSeverService = new(
        server.Client,
        new CompilationApplicationService(
            new LoadingBuiltinSymbolApplicationService(
                new LoadBuiltinSymbolInteractor(),
                symbolRepositories
            )
        )
    );


    protected override async Task Handle( DidOpenTextDocumentParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();

        await compilationSeverService.CompileAsync(
            compilationCacheManager: compilationCacheManager,
            scriptLocation: scriptLocation,
            script: request.TextDocument.Text,
            cancellationToken: token
        );
    }

    protected override async Task Handle( DidChangeTextDocumentParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var script = request.ContentChanges.First().Text;

        await compilationSeverService.CompileAsync(
            compilationCacheManager: compilationCacheManager,
            scriptLocation: scriptLocation,
            script: script,
            cancellationToken: token
        );
    }

    protected override async Task Handle( DidCloseTextDocumentParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();

        await compilationSeverService.ClearDiagnosticAsync( scriptLocation, token );
        compilationCacheManager.RemoveCache( scriptLocation );
    }

    protected override async Task Handle( WillSaveTextDocumentParams request, CancellationToken token )
    {
        await Task.CompletedTask;
    }

    protected override async Task<List<TextEdit>?> HandleRequest( WillSaveTextDocumentParams request, CancellationToken token )
    {
        await Task.CompletedTask;
        return [];
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.TextDocumentSync = new TextDocumentSyncOptions
        {
            Change    = TextDocumentSyncKind.Full,
            OpenClose = true,
            Save = new SaveOptions
            {
                IncludeText = true
            }
        };
    }
}
