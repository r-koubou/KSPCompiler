namespace KSPCompiler.Domain.Symbols;

public class KspPreProcessorSymbolTable : SymbolTable<KspPreProcessorSymbol>, IKspPreProcessorSymbolTable
{
    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    public KspPreProcessorSymbolTable()
        : base( null, UniqueSymbolIndex.Zero ) {}

    public KspPreProcessorSymbolTable( IKspPreProcessorSymbolTable? parent )
        : base( parent ) {}

    public KspPreProcessorSymbolTable( IKspPreProcessorSymbolTable? parent, UniqueSymbolIndex startUniqueIndex )
        : base( parent, startUniqueIndex ) {}
    // ReSharper restore MemberCanBePrivate.Global
    #endregion
}
