namespace KSPCompiler.Domain.Symbols;

public class PgsSymbolTable : SymbolTable<PgsSymbol>, IPgsSymbolTable
{
    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    public PgsSymbolTable()
        : base( null, UniqueSymbolIndex.Zero ) {}

    public PgsSymbolTable( IPgsSymbolTable? parent )
        : base( parent ) {}

    public PgsSymbolTable( IPgsSymbolTable? parent, UniqueSymbolIndex startUniqueIndex )
        : base( parent, startUniqueIndex ) {}
    // ReSharper restore MemberCanBePrivate.Global
    #endregion
}
