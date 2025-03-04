using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.Compilation.Domain.Symbols;

public sealed class PgsSymbol : SymbolBase, ISymbolDataTypeProvider
{
    #region Properties

    public override SymbolType Type
        => SymbolType.Pgs;

    ///
    /// <inheritdoc />
    ///
    public DataTypeFlag DataType { get; set; } = DataTypeFlag.None;

    #endregion ~ Properties

    // TODO Implementation
}
