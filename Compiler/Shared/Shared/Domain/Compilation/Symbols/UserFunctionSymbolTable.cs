namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public class UserFunctionSymbolTable : SymbolTable<UserFunctionSymbol>, IUserFunctionSymbolSymbolTable
{
    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    public UserFunctionSymbolTable()
        : base( null, UniqueSymbolIndex.Zero ) {}

    public UserFunctionSymbolTable( IUserFunctionSymbolSymbolTable? parent )
        : base( parent ) {}

    public UserFunctionSymbolTable( IUserFunctionSymbolSymbolTable? parent, UniqueSymbolIndex startUniqueIndex )
        : base( parent, startUniqueIndex ) {}
    // ReSharper restore MemberCanBePrivate.Global
    #endregion
}
