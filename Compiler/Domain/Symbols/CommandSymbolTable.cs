using System;

namespace KSPCompiler.Domain.Symbols;

public class CommandSymbolTable : SymbolTable<CommandSymbol>
{
    public override bool Add( CommandSymbol symbol )
    {
        if( table.ContainsKey( symbol.Name ) )
        {
            // Already added
            return false;
        }

        symbol.TableIndex = uniqueIndexGenerator.Next();
        table.Add( symbol.Name, symbol );

        return true;
    }
}
