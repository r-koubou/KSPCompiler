using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Domain.Symbols;

public sealed class UserFunctionSymbol : SymbolBase
{
    /// <summary>
    /// Symbol definition to end location information.
    /// </summary>
    public Position Range { get; set; } = Position.Zero;

    public override SymbolType Type
        => SymbolType.UserFunction;

    // TODO Implementation
}
