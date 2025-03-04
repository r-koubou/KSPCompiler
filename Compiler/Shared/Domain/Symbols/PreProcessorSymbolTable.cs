namespace KSPCompiler.Shared.Domain.Symbols;

public class PreProcessorSymbolTable : SymbolTable<PreProcessorSymbol>, IPreProcessorSymbolTable
{
    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    public PreProcessorSymbolTable()
        : base( null, UniqueSymbolIndex.Zero ) {}

    public PreProcessorSymbolTable( IPreProcessorSymbolTable? parent )
        : base( parent ) {}

    public PreProcessorSymbolTable( IPreProcessorSymbolTable? parent, UniqueSymbolIndex startUniqueIndex )
        : base( parent, startUniqueIndex ) {}
    // ReSharper restore MemberCanBePrivate.Global
    #endregion
}
