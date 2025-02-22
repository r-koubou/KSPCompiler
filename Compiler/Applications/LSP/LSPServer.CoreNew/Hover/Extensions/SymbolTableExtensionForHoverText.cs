using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.CoreNew.Hover.Extensions;

public static class SymbolTableExtensionForHoverText
{
    public static bool TryBuildHoverText<TSymbol>( this ISymbolTable<TSymbol> self, string symbolName, out string result, IHoverTextBuilder<TSymbol>? builder = null )
        where TSymbol : SymbolBase
    {
        result = string.Empty;

        if( !self.TrySearchByName( symbolName, out var symbol ) )
        {
            return false;
        }

        if( builder == null )
        {
            result = symbol.Description.Value;

            return true;
        }

        result = builder.BuildHoverText( symbol );

        return !string.IsNullOrEmpty( result );
    }
}
