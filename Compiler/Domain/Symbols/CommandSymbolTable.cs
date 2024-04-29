using System;

namespace KSPCompiler.Domain.Symbols;

public class CommandSymbolTable : SymbolTable<CommandSymbol>
{
    public override bool Add( CommandSymbol symbol )
    {
        throw new NotImplementedException();
    }
}
