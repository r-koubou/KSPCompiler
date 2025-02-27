using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server.Options;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Completion;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Completion.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.LanguageServer.Compilation;
using KSPCompiler.Interactors.LanguageServer.Completion;
using KSPCompiler.UseCases.LanguageServer.Completion;

using CompletionItem = EmmyLua.LanguageServer.Framework.Protocol.Message.Completion.CompletionItem;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Completion;

public class CompletionHandler(
    CompilationCacheManager compilationCacheManager
) : CompletionHandlerBase
{
    private readonly CompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly CompletionInteractor service = new();

    protected override async Task<CompletionResponse?> Handle( CompletionParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return null;
        }

        var input = new CompletionHandlingInputPort(
            new CompletionHandlingInputPortDetail( compilationCacheManager, scriptLocation, position )
        );
        var result = await service.ExecuteAsync( input, token );

        return new CompletionResponse( result.OutputData.As() );
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
