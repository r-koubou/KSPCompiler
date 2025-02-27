using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.Core.Extensions;

public static class ISymbolTableExtension
{
    public static bool TrySearchDefinitionLocation<TSymbol>(
        this ISymbolTable<TSymbol> symbolTable,
        ScriptLocation documentUri,
        string symbolName,
        out LocationLink result ) where TSymbol : SymbolBase
    {
        result = null!;

        if( !symbolTable.TrySearchByName( symbolName, out var symbol ) )
        {
            return false;
        }

        result = new LocationLink
        {
            Location = documentUri,
            Range    = symbol.DefinedPosition
        };

        return true;
    }
}
