using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public sealed class UserFunctionSymbol : SymbolBase
{
    public override SymbolType Type
        => SymbolType.UserFunction;
}
