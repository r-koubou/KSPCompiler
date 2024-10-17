using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public class KspPreProcessorSymbol : SymbolBase
{
    #region Properties

    public override SymbolType Type
        => SymbolType.KspPreprocessor;

    public override DataTypeFlag DataType
        => DataTypeFlag.TypeKspPreprocessorSymbol;

    #endregion ~ Properties

    // TODO Implementation
}
