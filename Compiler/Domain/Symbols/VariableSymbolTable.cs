using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Domain.Symbols;

public class VariableSymbolTable : SymbolTable<VariableSymbol>
{
    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    public VariableSymbolTable()
        : base( null, UniqueSymbolIndex.Zero ) {}

    public VariableSymbolTable( ISymbolTable<VariableSymbol>? parent )
        : base( parent ) {}

    public VariableSymbolTable( SymbolTable<VariableSymbol>? parent, UniqueSymbolIndex startUniqueIndex )
        : base( parent, startUniqueIndex ) {}
    // ReSharper restore MemberCanBePrivate.Global
    #endregion

    public override bool Add( VariableSymbol symbol )
    {
        if( table.ContainsKey( symbol.Name ) )
        {
            // Already added
            return false;
        }

        if( symbol.DataTypeModifier.IsConstant() )
        {
            symbol.TableIndex = UniqueSymbolIndex.Null;
        }
        else
        {
            symbol.TableIndex = uniqueIndexGenerator.Next();
        }

        symbol.DataType = DataTypeUtility.FromVariableName( symbol.Name );
        table.Add( symbol.Name, symbol );

        return true;
    }
}
