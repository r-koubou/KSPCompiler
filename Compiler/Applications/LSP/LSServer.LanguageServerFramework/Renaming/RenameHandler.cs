using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Rename;
using EmmyLua.LanguageServer.Framework.Protocol.Model;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Renaming.Extensions;
using KSPCompiler.Interactors.LanguageServer.Compilation;
using KSPCompiler.Interactors.LanguageServer.Renaming;
using KSPCompiler.UseCases.LanguageServer.Renaming;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Renaming;

public sealed class RenameHandler( CompilationCacheManager compilationCacheManager ) : RenameHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly RenamingInteractor interactor = new();

    protected override async Task<WorkspaceEdit?> Handle( RenameParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();
        var newName = request.NewName;

        var input = new RenamingInputPort(
            new RenamingInputPortDetail( compilationCacheManager, scriptLocation, position, newName )
        );

        var result = await interactor.ExecuteAsync( input, token );

        if( !result.Result || result.OutputData.Count == 0 )
        {
            return null;
        }

        return new WorkspaceEdit
        {
            Changes = result.OutputData.As()
        };
    }

    protected override async Task<PrepareRenameResponse> Handle( PrepareRenameParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        var result = await interactor.HandlePrepareAsync( compilationCacheManager, scriptLocation, position, token );

        return !result.result
            ? new PrepareRenameResponse( defaultBehavior: false )
            : new PrepareRenameResponse( result.position.AsRange() );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.RenameProvider = true;
    }
}
