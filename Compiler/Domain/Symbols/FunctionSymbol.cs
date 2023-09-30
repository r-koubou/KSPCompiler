using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public sealed class FunctionSymbol : SymbolBase
{
    public override SymbolType Type
        => SymbolType.UserFunction;
}
