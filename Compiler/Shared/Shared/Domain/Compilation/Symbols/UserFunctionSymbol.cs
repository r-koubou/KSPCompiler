using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public sealed record UserFunctionSymbol : SymbolBase
{
    /// <summary>
    /// Symbol definition to end location information.
    /// </summary>
    public Position Range { get; set; } = Position.Zero;

    public override SymbolType Type
        => SymbolType.UserFunction;

    // TODO Implementation
}
