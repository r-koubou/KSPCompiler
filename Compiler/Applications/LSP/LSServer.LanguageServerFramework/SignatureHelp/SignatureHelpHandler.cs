using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server.Options;
using EmmyLua.LanguageServer.Framework.Protocol.Message.SignatureHelp;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.SignatureHelp.Extensions;
using KSPCompiler.Interactors.LanguageServer.SignatureHelp;
using KSPCompiler.UseCases.LanguageServer.Compilation;
using KSPCompiler.UseCases.LanguageServer.SignatureHelp;

using FrameworkSignatureHelp = EmmyLua.LanguageServer.Framework.Protocol.Message.SignatureHelp.SignatureHelp;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.SignatureHelp;

public class SignatureHelpHandler( ICompilationCacheManager compilationCacheManager ) : SignatureHelpHandlerBase
{
    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly SignatureHelpInteractor interactor = new();

    protected override async Task<FrameworkSignatureHelp> Handle( SignatureHelpParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        var input = new SignatureHelpInputPort(
            new SignatureHelpInputPortDetail(
                compilationCacheManager,
                scriptLocation,
                position
            )
        );

        var output = await interactor.ExecuteAsync( input, token );

        if( !output.Result || output.OutputData == null )
        {
            return new FrameworkSignatureHelp
            {
                Signatures = [ ]
            };
        }

        return output.OutputData.As();
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        serverCapabilities.SignatureHelpProvider = new SignatureHelpOptions
        {
            TriggerCharacters   = [ "(", "," ],
            RetriggerCharacters = [ "," ]
        };
    }
}
