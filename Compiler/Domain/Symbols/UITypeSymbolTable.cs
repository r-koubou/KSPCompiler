using System;

namespace KSPCompiler.Domain.Symbols;

public class UITypeSymbolTable : SymbolTable<UITypeSymbol>
{
    public override bool Add( SymbolName name, UITypeSymbol symbol )
    {
        throw new NotImplementedException();
    }
}
