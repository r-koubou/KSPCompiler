namespace KSPCompiler.Domain.Symbols;

public class FunctionSymbolTable : SymbolTable<FunctionSymbol>
{
    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    public FunctionSymbolTable()
        : base( null, UniqueSymbolIndex.Zero ) {}

    public FunctionSymbolTable( SymbolTable<FunctionSymbol>? parent )
        : base( parent ) {}

    public FunctionSymbolTable( SymbolTable<FunctionSymbol>? parent, UniqueSymbolIndex startUniqueIndex )
        : base( parent, startUniqueIndex ) {}
    // ReSharper restore MemberCanBePrivate.Global
    #endregion

    public override bool Add( SymbolName name, FunctionSymbol symbol )
        => throw new System.NotImplementedException();

    public override object Clone()
        => throw new System.NotImplementedException();
}
