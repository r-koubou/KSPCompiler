namespace KSPCompiler.Domain.Symbols;

public class CallbackSymbolTable : OverloadedSymbolTable<CallbackSymbol, SymbolName>
{
    public CallbackSymbolTable(
        IOverloadedSymbolTable<CallbackSymbol, SymbolName>? parent = null
    ) : base( parent ) {}

    public CallbackSymbolTable(
        UniqueSymbolIndex startUniqueIndex,
        IOverloadedSymbolTable<CallbackSymbol, SymbolName>? parent = null
    ) : base( startUniqueIndex, parent ) {}

    public override SymbolName NoOverloadValue
        => SymbolName.Empty;
}
