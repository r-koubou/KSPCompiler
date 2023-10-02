using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public sealed class UITypeSymbol : SymbolBase
{
    public override SymbolType Type
        => SymbolType.Variable;

    public UITypeSymbol() {}
}
