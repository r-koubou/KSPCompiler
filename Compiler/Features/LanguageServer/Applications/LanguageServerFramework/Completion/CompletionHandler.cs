using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server.Options;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Completion;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Completion.Extensions;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Features.LanguageServer.UseCase.Completion;
using KSPCompiler.Shared.Domain.Symbols.MetaData;

using FrameworkCompletionItem = EmmyLua.LanguageServer.Framework.Protocol.Message.Completion.CompletionItem;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Completion;

public class CompletionHandler(
    ICompilationCacheManager compilationCacheManager
) : CompletionHandlerBase
{
    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly CompletionInteractor interactor = new();

    protected override async Task<CompletionResponse?> Handle( CompletionParams request, CancellationToken token )
    {
        var scriptLocation = request.TextDocument.Uri.AsScriptLocation();
        var position = request.Position.As();

        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return null;
        }

        var input = new CompletionHandlingInputPort(
            new CompletionHandlingInputPortDetail(
                compilationCacheManager,
                scriptLocation,
                position
            )
        );

        var output = await interactor.ExecuteAsync( input, token );

        if( !output.Result || output.OutputData.Count == 0 )
        {
            return null;
        }

        return new CompletionResponse( output.OutputData.As() );
    }

    protected override async Task<FrameworkCompletionItem> Resolve( FrameworkCompletionItem item, CancellationToken token )
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
