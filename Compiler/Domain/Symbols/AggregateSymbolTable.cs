namespace KSPCompiler.Domain.Symbols;

public sealed class AggregateSymbolTable
{
    public AggregateVariableSymbolTable Variables { get; }

    public IVariableSymbolTable BuiltInVariables
        => Variables.BuiltIn;

    public IVariableSymbolTable UserVariables
        => Variables.User;

    public IUITypeSymbolTable UITypes { get; }

    public ICommandSymbolTable Commands { get; }

    public AggregateCallbackSymbolTable Callbacks { get; }

    public ICallbackSymbolTable BuiltInCallbacks
        => Callbacks.BuiltIn;

    public ICallbackSymbolTable UserCallbacks
        => Callbacks.User;

    public IUserFunctionSymbolSymbolTable UserFunctions { get; }

    public IPreProcessorSymbolTable PreProcessorSymbols { get; }

    public AggregateSymbolTable(
        AggregateVariableSymbolTable variables,
        IUITypeSymbolTable uiTypes,
        ICommandSymbolTable commands,
        AggregateCallbackSymbolTable callbacks,
        IUserFunctionSymbolSymbolTable userFunctions,
        IPreProcessorSymbolTable preProcessorSymbols )
    {
        Variables           = variables;
        UITypes             = uiTypes;
        Commands            = commands;
        Callbacks           = callbacks;
        UserFunctions       = userFunctions;
        PreProcessorSymbols = preProcessorSymbols;
    }
}
