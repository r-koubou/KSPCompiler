namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public class CallbackSymbolTable
    : OverloadedSymbolTable<CallbackSymbol, CallbackArgumentSymbolList>,
    ICallbackSymbolTable
{
    public CallbackSymbolTable() : this( null ) {}

    public CallbackSymbolTable(
        IOverloadedSymbolTable<CallbackSymbol, CallbackArgumentSymbolList>? parent = null
    ) : base( parent ) {}

    public CallbackSymbolTable(
        UniqueSymbolIndex startUniqueIndex,
        IOverloadedSymbolTable<CallbackSymbol, CallbackArgumentSymbolList>? parent = null
    ) : base( startUniqueIndex, parent ) {}

    public override CallbackArgumentSymbolList NoOverloadValue
        => CallbackArgumentSymbolList.Null;
}
