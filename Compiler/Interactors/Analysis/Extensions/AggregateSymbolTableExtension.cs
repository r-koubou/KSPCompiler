using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Interactors.Analysis.Extensions;

public static class AggregateSymbolTableExtension
{
    #region Variables

    public static bool TrySearchVariableByName( this AggregateSymbolTable symbolTable, SymbolName name, out VariableSymbol result, bool enableSearchParent = true )
    {
        if( symbolTable.BuiltInVariables.TrySearchByName( name, out result, enableSearchParent ) )
        {
            return true;
        }

        if( symbolTable.UserVariables.TrySearchByName( name, out result, enableSearchParent ) )
        {
            return true;
        }

        return false;
    }

    public static bool TrySearchBuiltInVariableByName( this AggregateSymbolTable symbolTable, SymbolName name, out VariableSymbol result, bool enableSearchParent = true )
    {
        return symbolTable.BuiltInVariables.TrySearchByName( name, out result, enableSearchParent );
    }

    public static bool TrySearchUserVariableByName( this AggregateSymbolTable symbolTable, SymbolName name, out VariableSymbol result, bool enableSearchParent = true )
    {
        return symbolTable.UserVariables.TrySearchByName( name, out result, enableSearchParent );
    }

    #endregion ~Variables

    public static bool TrySearchUserFunctionByName( this AggregateSymbolTable symbolTable, SymbolName name, out UserFunctionSymbol result, bool enableSearchParent = true )
    {
        return symbolTable.UserFunctions.TrySearchByName( name, out result, enableSearchParent );
    }

    public static bool TrySearchCommandByName( this AggregateSymbolTable symbolTable, SymbolName name, out CommandSymbol result, bool enableSearchParent = true )
    {
        return symbolTable.Commands.TrySearchByName( name, out result, enableSearchParent );
    }

    public static bool TrySearchUITypeByName( this AggregateSymbolTable symbolTable, SymbolName name, out UITypeSymbol result, bool enableSearchParent = true )
    {
        return symbolTable.UITypes.TrySearchByName( name, out result, enableSearchParent );
    }
}
