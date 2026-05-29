using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server.Options;
using EmmyLua.LanguageServer.Framework.Protocol.Message.SignatureHelp;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.SignatureHelp.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.SignatureHelp;
using KSPCompiler.Features.LanguageServer.UseCase.SignatureHelp;

using FrameworkSignatureHelp = EmmyLua.LanguageServer.Framework.Protocol.Message.SignatureHelp.SignatureHelp;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.SignatureHelp;

public class SignatureHelpHandler( ICompilationCacheManager compilationCacheManager ) : SignatureHelpHandlerBase
{
    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly SignatureHelpInteractor interactor = new();

    protected override async Task<FrameworkSignatureHelp> Handle( SignatureHelpParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        var input = new SignatureHelpInput(
            compilationCacheManager,
            scriptLocation,
            position
        );

        var result = await interactor.ExecuteAsync( input, token );

        if( result.IsFailure )
        {
            return new FrameworkSignatureHelp
            {
                Signatures = [ ]
            };
        }

        var output = result.Unwrap();

        if( output.SignatureHelp == null )
        {
            return new FrameworkSignatureHelp
            {
                Signatures = [ ]
            };
        }

        return output.SignatureHelp.As();
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
