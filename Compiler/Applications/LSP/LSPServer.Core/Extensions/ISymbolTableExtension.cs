using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Extensions;

public static class ISymbolTableExtension
{
    public static void RemoveNoReservedSymbols<T>( this ISymbolTable<T> self ) where T : SymbolBase
    {
        var removeList = new List<T>();

        foreach( var symbol in self.Table.Values )
        {
            if( !symbol.BuiltIn )
            {
                removeList.Add( symbol );
            }
        }

        foreach( var x in removeList )
        {
            self.Remove( x );
        }
    }

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
