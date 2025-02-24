using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server.Options;
using EmmyLua.LanguageServer.Framework.Protocol.Message.SignatureHelp;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSPServer.Core.Compilation;
using KSPCompiler.Applications.LSPServer.Core.SignatureHelp;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Hover.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.SignatureHelp.Extensions;

using FrameworkSignatureHelp = EmmyLua.LanguageServer.Framework.Protocol.Message.SignatureHelp.SignatureHelp;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.SignatureHelp;

public class SignatureHelpHandler( CompilationCacheManager compilationCacheManager ) : SignatureHelpHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly SignatureHelpHandlingService service = new();

    protected override async Task<FrameworkSignatureHelp> Handle( SignatureHelpParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();
        var result = await service.HandleAsync( compilationCacheManager, scriptLocation, position, token );

        if( result == null )
        {
            return new FrameworkSignatureHelp
            {
                Signatures = [ ]
            };
        }

        return result.As();
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
