using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Rename;
using EmmyLua.LanguageServer.Framework.Protocol.Model;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Applications.LSPServer.CoreNew.Renaming;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Renaming.Extensions;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Renaming;

public sealed class RenameHandler( CompilationCacheManager compilationCacheManager ) : RenameHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly RenameHandlingService service = new();

    protected override async Task<WorkspaceEdit?> Handle( RenameParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();
        var newName = request.NewName;

        var result = await service.HandleAsync(
            compilationCacheManager,
            scriptLocation,
            position,
            newName,
            token
        );

        if( result.Count == 0 )
        {
            return null;
        }

        return new WorkspaceEdit
        {
            Changes = result.As()
        };
    }

    protected override async Task<PrepareRenameResponse> Handle( PrepareRenameParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        var result = await service.HandlePrepareAsync( compilationCacheManager, scriptLocation, position, token );

        return !result.result
            ? new PrepareRenameResponse( defaultBehavior: false )
            : new PrepareRenameResponse( result.position.AsRange() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.RenameProvider = true;
    }
}
