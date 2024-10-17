using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public class PgsSymbol : SymbolBase
{
    #region Properties

    public override SymbolType Type
        => SymbolType.Pgs;

    public override DataTypeFlag DataType
        => DataTypeFlag.TypePgsId;

    #endregion ~ Properties

    // TODO Implementation
}
