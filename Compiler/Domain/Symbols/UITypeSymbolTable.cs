namespace KSPCompiler.Domain.Symbols;

public class UITypeSymbolTable : SymbolTable<UITypeSymbol>
{
    public override bool Add( SymbolName name, UITypeSymbol symbol )
    {
        throw new System.NotImplementedException();
    }

    public override void Merge( ISymbolTable<UITypeSymbol> other )
    {
        throw new System.NotImplementedException();
    }

    public override object Clone()
    {
        throw new System.NotImplementedException();
    }
}
