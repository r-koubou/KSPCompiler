using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.LSPServer.Core.Compilations;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Completions;

public class CompletionHandler : ICompletionHandler
{
    private readonly CompletionRegistrationOptions options;

    private CompilerCacheService CompilerCacheService { get; }
    private CompletionListService CompletionListService { get; }

    public CompletionHandler(
        CompilerCacheService compilerCacheService,
        CompletionListService completionListService )
    {
        CompilerCacheService         = compilerCacheService;
        CompletionListService = completionListService;

        var triggerCharacters = new List<string>();
        triggerCharacters.AddRange( DataTypeUtility.KspTypeCharactersAsString );
        triggerCharacters.Add( "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_0123456789" );

        options = new CompletionRegistrationOptions
        {
            DocumentSelector  = ConstantValues.TextDocumentSelector,
            ResolveProvider   = true,
            TriggerCharacters = new Container<string>( triggerCharacters )
        };
    }

    public async Task<CompletionList> Handle( CompletionParams request, CancellationToken cancellationToken )
        => await CompletionListService.HandleAsync( CompilerCacheService, request, cancellationToken );

    public CompletionRegistrationOptions GetRegistrationOptions( CompletionCapability capability, ClientCapabilities clientCapabilities )
        => options;
}
