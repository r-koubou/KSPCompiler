using KSPCompiler.Shared.Domain.Symbols.MetaData;

namespace KSPCompiler.Shared.Domain.Symbols;

public sealed class PreProcessorSymbol : SymbolBase, ISymbolDataTypeProvider
{
    #region Properties

    public override SymbolType Type
        => SymbolType.Preprocessor;

    ///
    /// <inheritdoc />
    ///
    public DataTypeFlag DataType { get; set; } = DataTypeFlag.None;

    #endregion ~ Properties

    // TODO Implementation
}
