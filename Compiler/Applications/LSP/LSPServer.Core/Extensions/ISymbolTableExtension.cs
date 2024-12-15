using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

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
}
