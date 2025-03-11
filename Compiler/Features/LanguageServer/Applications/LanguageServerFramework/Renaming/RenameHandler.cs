using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Rename;
using EmmyLua.LanguageServer.Framework.Protocol.Model;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Renaming.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Renaming;
using KSPCompiler.Features.LanguageServer.UseCase.Renaming;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Renaming;

public sealed class RenameHandler( ICompilationCacheManager compilationCacheManager ) : RenameHandlerBase
{
    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly RenamingInteractor interactor = new();

    protected override async Task<WorkspaceEdit?> Handle( RenameParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();
        var newName = request.NewName;

        var input = new RenamingInputPort(
            new RenamingInputPortDetail(
                compilationCacheManager,
                scriptLocation,
                position,
                newName
            )
        );

        var output = await interactor.ExecuteAsync( input, token );

        if( !output.Result || output.OutputData.Count == 0 )
        {
            return null;
        }

        return new WorkspaceEdit
        {
            Changes = output.OutputData.As()
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
