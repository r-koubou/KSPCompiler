using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.Extensions;

public static class ISymbolTableExtension
{
    public static bool TrySearchDefinitionLocation<TSymbol>(
        this ISymbolTable<TSymbol> symbolTable,
        DocumentUri documentUri,
        string symbolName,
        out Location result ) where TSymbol : SymbolBase
    {
        result = null!;

        if( !symbolTable.TrySearchByName( symbolName, out var symbol ) )
        {
            return false;
        }

        result = new Location
        {
            Uri = documentUri,
            Range = new Range()
            {
                Start = symbol.DefinedPosition.BeginAs(),
                End   = symbol.DefinedPosition.EndAs()
            }
        };

        return true;
    }
}
