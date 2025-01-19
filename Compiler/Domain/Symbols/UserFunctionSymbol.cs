using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public sealed record UserFunctionSymbol : SymbolBase
{
    public override SymbolType Type
        => SymbolType.UserFunction;

    // TODO Implementation
}
