namespace KSPCompiler.Domain.Symbols;

public sealed class AggregateSymbolTable
{
    public IVariableSymbolTable BuiltInVariables { get; }
    public IVariableSymbolTable UserVariables { get; }
    public IUITypeSymbolTable UITypes { get; }
    public ICommandSymbolTable Commands { get; }
    public ICallbackSymbolTable UserCallbacks { get; }
    public ICallbackSymbolTable BuiltInCallbacks { get; }
    public IUserFunctionSymbolSymbolTable UserFunctions { get; }
    public IPreProcessorSymbolTable PreProcessorSymbols { get; }

    public AggregateSymbolTable(
        IVariableSymbolTable builtInVariables,
        IVariableSymbolTable userVariables,
        IUITypeSymbolTable uiTypes,
        ICommandSymbolTable commands,
        ICallbackSymbolTable builtInCallbacks,
        ICallbackSymbolTable userCallbacks,
        IUserFunctionSymbolSymbolTable userFunctions,
        IPreProcessorSymbolTable preProcessorSymbols )
    {
        BuiltInVariables    = builtInVariables;
        UserVariables       = userVariables;
        UITypes             = uiTypes;
        Commands            = commands;
        UserCallbacks       = userCallbacks;
        BuiltInCallbacks    = builtInCallbacks;
        UserFunctions       = userFunctions;
        PreProcessorSymbols = preProcessorSymbols;
    }
}
