using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public class PreProcessorSymbol : SymbolBase
{
    #region Properties

    public override SymbolType Type
        => SymbolType.Preprocessor;

    public override DataTypeFlag DataType
        => DataTypeFlag.TypePreprocessorSymbol;

    #endregion ~ Properties

    // TODO Implementation
}
