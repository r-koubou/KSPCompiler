using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Compilations;
using KSPCompiler.LSPServer.Core.Hovers;
using KSPCompiler.LSPServer.Core.Hovers.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SignatureHelps;

public sealed class SignatureHelpService( CompilerCacheService compilerCacheService )
{
    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;

    public async Task<SignatureHelp?> HandleAsync( SignatureHelpParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var symbols = cache.SymbolTable;
        var word = DocumentUtility.ExtractCallCommandName( cache.AllLinesText, request.Position );

        if( string.IsNullOrEmpty( word ) )
        {
            return null;
        }

        Console.WriteLine( word );

        if( !symbols.Commands.TryBuildHoverText( word, out var hoverText, new CommandHoverTextBuilder() ) )
        {
            return null;
        }

        var signatureInformation = new SignatureInformation
        {
            Label = word,
            Documentation = new StringOrMarkupContent(
                new MarkupContent
                {
                    Kind  = MarkupKind.Markdown,
                    Value = hoverText
                }
            )
        };

        await Task.CompletedTask;

        return new SignatureHelp
        {
            Signatures = new Container<SignatureInformation>( signatureInformation ),
        };
    }
}
