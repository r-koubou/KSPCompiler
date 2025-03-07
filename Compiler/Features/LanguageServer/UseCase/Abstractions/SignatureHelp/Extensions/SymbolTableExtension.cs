using System.Linq;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.SignatureHelp.Extensions;

public static class SymbolTableExtension
{
    private static ISignatureHelpBuilder<CommandSymbol> DefaultBuilder { get; }
        = new DefaultSignatureHelpBuilder();

    public static bool TryBuildSignatureHelp(
        this ICommandSymbolTable self,
        string symbolName,
        int activeParameter,
        out SignatureHelpItem result,
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
