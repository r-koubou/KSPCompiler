namespace KSPCompiler.Domain.Symbols;

public class UserFunctionSymbolTable : SymbolTable<UserFunctionSymbol>
{
    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    public UserFunctionSymbolTable()
        : base( null, UniqueSymbolIndex.Zero ) {}

    public UserFunctionSymbolTable( SymbolTable<UserFunctionSymbol>? parent )
        : base( parent ) {}

    public UserFunctionSymbolTable( SymbolTable<UserFunctionSymbol>? parent, UniqueSymbolIndex startUniqueIndex )
        : base( parent, startUniqueIndex ) {}
    // ReSharper restore MemberCanBePrivate.Global
    #endregion

    public override bool Add( SymbolName name, UserFunctionSymbol symbol )
    {
        throw new System.NotImplementedException();
    }

    public override void Merge( ISymbolTable<UserFunctionSymbol> other )
    {
        throw new System.NotImplementedException();
    }
}
