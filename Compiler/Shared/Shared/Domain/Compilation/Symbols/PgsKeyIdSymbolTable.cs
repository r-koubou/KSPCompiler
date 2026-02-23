namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public class PgsKeyIdSymbolTable : SymbolTable<PgsSymbol>, IPgsKeyIdSymbolTable
{
    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    public PgsKeyIdSymbolTable()
        : base( null, UniqueSymbolIndex.Zero ) {}

    public PgsKeyIdSymbolTable( IPgsKeyIdSymbolTable? parent )
        : base( parent ) {}

    public PgsKeyIdSymbolTable( IPgsKeyIdSymbolTable? parent, UniqueSymbolIndex startUniqueIndex )
        : base( parent, startUniqueIndex ) {}
    // ReSharper restore MemberCanBePrivate.Global
    #endregion
}
