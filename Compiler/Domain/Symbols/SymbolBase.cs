using System;

using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public abstract class SymbolBase
{
    public SymbolName Name { get; set; } = SymbolName.Empty;
    public bool IsReserved { get; set; } = false;
    public abstract SymbolType Type { get; }
    public DataTypeFlag DataType { get; set; } = DataTypeFlag.None;
    public DataModifierFlag DataModifier { get; set; } = DataModifierFlag.None;
    public UniqueSymbolIndex TableIndex { get; set; } = UniqueSymbolIndex.Null;
}
