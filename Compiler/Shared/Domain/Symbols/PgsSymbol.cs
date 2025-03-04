using KSPCompiler.Shared.Domain.Symbols.MetaData;

namespace KSPCompiler.Shared.Domain.Symbols;

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
