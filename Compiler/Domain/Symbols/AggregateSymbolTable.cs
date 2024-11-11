namespace KSPCompiler.Domain.Symbols;

public sealed class AggregateSymbolTable
{
    public IVariableSymbolTable Variables { get; }
    public IUITypeSymbolTable UITypes { get; }
    public ICommandSymbolTable Commands { get; }
    public ICallbackSymbolTable UserCallbacks { get; }
    public ICallbackSymbolTable BuiltInCallbacks { get; }
    public IUserFunctionSymbolSymbolTable UserFunctions { get; }
    public IPreProcessorSymbolTable PreProcessorSymbols { get; }

    public AggregateSymbolTable(
        IVariableSymbolTable variables,
        IUITypeSymbolTable uiTypes,
        ICommandSymbolTable commands,
        ICallbackSymbolTable userCallbacks,
        ICallbackSymbolTable builtInCallbacks,
        IUserFunctionSymbolSymbolTable userFunctions,
        IPreProcessorSymbolTable preProcessorSymbols )
    {
        Variables           = variables;
        UITypes             = uiTypes;
        Commands            = commands;
        UserCallbacks       = userCallbacks;
        BuiltInCallbacks    = builtInCallbacks;
        UserFunctions       = userFunctions;
        PreProcessorSymbols = preProcessorSymbols;
    }
}
