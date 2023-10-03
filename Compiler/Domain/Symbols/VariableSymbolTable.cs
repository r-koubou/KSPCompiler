using System;

namespace KSPCompiler.Domain.Symbols;

public class VariableSymbolTable : SymbolTable<VariableSymbol>
{
    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    public VariableSymbolTable()
        : base( null, UniqueSymbolIndex.Zero ) {}

    public VariableSymbolTable( SymbolTable<VariableSymbol>? parent )
        : base( parent ) {}

    public VariableSymbolTable( SymbolTable<VariableSymbol>? parent, UniqueSymbolIndex startUniqueIndex )
        : base( parent, startUniqueIndex ) {}
    // ReSharper restore MemberCanBePrivate.Global
    #endregion

    public override bool Add( SymbolName name, VariableSymbol symbol )
    {
        throw new NotImplementedException();
    }
}
