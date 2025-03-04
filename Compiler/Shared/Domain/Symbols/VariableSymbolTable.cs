using KSPCompiler.Shared.Domain.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Symbols.MetaData.Extensions;

namespace KSPCompiler.Shared.Domain.Symbols;

public class VariableSymbolTable : SymbolTable<VariableSymbol>, IVariableSymbolTable
{
    #region ctor
    // ReSharper disable MemberCanBePrivate.Global
    public VariableSymbolTable()
        : base( null, UniqueSymbolIndex.Zero ) {}

    public VariableSymbolTable( IVariableSymbolTable? parent )
        : base( parent ) {}

    public VariableSymbolTable( IVariableSymbolTable? parent, UniqueSymbolIndex startUniqueIndex )
        : base( parent, startUniqueIndex ) {}
    // ReSharper restore MemberCanBePrivate.Global
    #endregion

    protected override void OnWillAdd( VariableSymbol symbol )
    {
        if( symbol.Modifier.IsConstant() )
        {
            symbol.TableIndex = UniqueSymbolIndex.Null;
        }
        else
        {
            symbol.TableIndex = uniqueIndexGenerator.Next();
        }

        symbol.DataType = DataTypeUtility.GuessFromSymbolName( symbol.Name );
    }
}
