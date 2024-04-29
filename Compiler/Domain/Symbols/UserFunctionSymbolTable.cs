using System;

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

    public override bool Add( UserFunctionSymbol symbol )
    {
        throw new NotImplementedException();
    }
}
