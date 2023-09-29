namespace KSPCompiler.Domain.Symbols;

public abstract class SymbolBase
{
    public SymbolType SymbolType { get; set; } = SymbolType.Unknown;
    public SymbolName Name { get; set; } = SymbolName.Empty;
    public UniqueSymbolIndex TableIndex { get; set; } = UniqueSymbolIndex.Null;
}
