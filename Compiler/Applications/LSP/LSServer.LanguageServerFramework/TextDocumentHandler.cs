using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server.Options;
using EmmyLua.LanguageServer.Framework.Protocol.Message.TextDocument;
using EmmyLua.LanguageServer.Framework.Protocol.Model.TextEdit;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Interactors.ApplicationServices.Compilation;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework;

public sealed class TextDocumentHandler( CompilationCacheManager compilationCacheManager ) : TextDocumentHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly CompilationApplicationService service = new();

    protected override Task Handle( DidOpenTextDocumentParams request, CancellationToken token )
    {
        throw new System.NotImplementedException();
    }

    protected override Task Handle( DidChangeTextDocumentParams request, CancellationToken token )
    {
        throw new System.NotImplementedException();
    }

    protected override Task Handle( DidCloseTextDocumentParams request, CancellationToken token )
    {
        throw new System.NotImplementedException();
    }

    protected override Task Handle( WillSaveTextDocumentParams request, CancellationToken token )
    {
        throw new System.NotImplementedException();
    }

    protected override Task<List<TextEdit>?> HandleRequest( WillSaveTextDocumentParams request, CancellationToken token )
    {
        throw new System.NotImplementedException();
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.TextDocumentSync = new TextDocumentSyncOptions
        {
            Change = TextDocumentSyncKind.Full,
            OpenClose = true,
            Save = new SaveOptions { IncludeText = true }
        };
    }
}
