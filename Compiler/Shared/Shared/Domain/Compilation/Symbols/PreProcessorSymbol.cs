using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public sealed record PreProcessorSymbol : SymbolBase, ISymbolDataTypeProvider
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
