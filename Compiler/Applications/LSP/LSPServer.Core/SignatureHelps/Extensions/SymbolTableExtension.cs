using System.Linq;

using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Core.SignatureHelps.Extensions;

public static class SymbolTableExtension
{
    private static ISignatureHelpBuilder<CommandSymbol> DefaultBuilder { get; }
        = new DefaultSignatureHelpBuilder();

    public static bool TryBuildSignatureHelp(
        this ICommandSymbolTable self,
        string symbolName,
        int activeParameter,
        out SignatureHelp result,
        ISignatureHelpBuilder<CommandSymbol>? builder = null )
    {
        result = null!;

        if( !self.TrySearchByName( symbolName, out var symbol ) )
        {
            return false;
        }

        builder ??= DefaultBuilder;
        result  =   builder.Build( symbol, activeParameter );

        return result.Signatures.Any();
    }
}
