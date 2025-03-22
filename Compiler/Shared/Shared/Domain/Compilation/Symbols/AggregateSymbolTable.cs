using System;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public sealed class AggregateSymbolTable
{
    public IVariableSymbolTable BuiltInVariables { get; }
    public IVariableSymbolTable UserVariables { get; }
    public IUITypeSymbolTable UITypes { get; }
    public ICommandSymbolTableNew CommandsNew { get; }
    public ICallbackSymbolTable UserCallbacks { get; }
    public ICallbackSymbolTable BuiltInCallbacks { get; }
    public IUserFunctionSymbolSymbolTable UserFunctions { get; }
    public IPreProcessorSymbolTable PreProcessorSymbols { get; }

    public AggregateSymbolTable(
        IVariableSymbolTable? builtInVariables = null,
        IVariableSymbolTable? userVariables = null,
        IUITypeSymbolTable? uiTypes = null,
        ICommandSymbolTableNew? commandsNew = null,
        ICallbackSymbolTable? builtInCallbacks = null,
        ICallbackSymbolTable? userCallbacks = null,
        IUserFunctionSymbolSymbolTable? userFunctions = null,
        IPreProcessorSymbolTable? preProcessorSymbols = null )
    {
        BuiltInVariables    = builtInVariables ?? new VariableSymbolTable();
        UserVariables       = userVariables ?? new VariableSymbolTable();
        UITypes             = uiTypes ?? new UITypeSymbolTable();
        CommandsNew         = commandsNew ?? new CommandSymbolTableNew();
        UserCallbacks       = userCallbacks ?? new CallbackSymbolTable();
        BuiltInCallbacks    = builtInCallbacks ?? new CallbackSymbolTable();
        UserFunctions       = userFunctions ?? new UserFunctionSymbolTable();
        PreProcessorSymbols = preProcessorSymbols ?? new PreProcessorSymbolTable();
    }

    public void Clear()
    {
        BuiltInVariables.Clear();
        UserVariables.Clear();
        UITypes.Clear();
        CommandsNew.Clear();
        UserCallbacks.Clear();
        BuiltInCallbacks.Clear();
        UserFunctions.Clear();
        PreProcessorSymbols.Clear();
    }

    public static void Merge( AggregateSymbolTable source, AggregateSymbolTable target )
    {
        // Variables
        // UI Type
        // User Function
        // PreProcessor Symbol
        target.BuiltInVariables.AddRange( source.BuiltInVariables );
        target.UserVariables.AddRange( source.UserVariables );
        target.UITypes.AddRange( source.UITypes );
        target.UserFunctions.AddRange( source.UserFunctions );

        // Command
        foreach( var x in source.CommandsNew )
        {
            foreach( var symbol in x.Values )
            {
                target.CommandsNew.AddAsOverload( symbol, symbol.Arguments );
            }
        }

        // Callback
        foreach( var x in source.BuiltInCallbacks )
        {
            foreach( var symbol in x.Values )
            {
                if( symbol.AllowMultipleDeclaration )
                {
                    target.BuiltInCallbacks.AddAsOverload( symbol, symbol.Arguments );
                }
                else
                {
                    target.BuiltInCallbacks.AddAsNoOverload( symbol );
                }
            }
        }
    }
}
