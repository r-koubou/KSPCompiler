using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server.Options;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Completion;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSPServer.Core.Compilation;
using KSPCompiler.Applications.LSPServer.Core.Completion;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Completion.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;

using CompletionItem = EmmyLua.LanguageServer.Framework.Protocol.Message.Completion.CompletionItem;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Completion;

public class CompletionHandler(
    CompilationCacheManager compilationCacheManager
) : CompletionHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly CompletionListHandlingService service = new();

    protected override async Task<CompletionResponse?> Handle( CompletionParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return null;
        }

        var result = await service.HandleAsync( compilationCacheManager, scriptLocation, position, token );

        return new CompletionResponse( result.As() );
    }

    protected override async Task<CompletionItem> Resolve( CompletionItem item, CancellationToken token )
    {
        await Task.CompletedTask;

        return item;
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities )
    {
        var triggerCharacters = new List<string>();
        triggerCharacters.AddRange( DataTypeUtility.KspTypeCharactersAsString );
        triggerCharacters.Add( "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_0123456789" );

        serverCapabilities.CompletionProvider = new CompletionOptions
        {
            ResolveProvider   = true,
            TriggerCharacters = triggerCharacters
        };
    }
}
