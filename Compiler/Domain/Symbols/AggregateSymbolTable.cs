namespace KSPCompiler.Domain.Symbols;

public sealed class AggregateSymbolTable
{
    public IVariableSymbolTable Variables { get; }
    public IUITypeSymbolTable UITypes { get; }
    public ICommandSymbolTable Commands { get; }
    public ICallbackSymbolTable UserCallbacks { get; }
    public ICallbackSymbolTable ReservedCallbacks { get; }
    public IUserFunctionSymbolSymbolTable UserFunctions { get; }
    public IKspPreProcessorSymbolTable PreProcessorSymbols { get; }
    public IPgsSymbolTable PgsSymbols { get; }

    public AggregateSymbolTable(
        IVariableSymbolTable variables,
        IUITypeSymbolTable uiTypes,
        ICommandSymbolTable commands,
        ICallbackSymbolTable userCallbacks,
        ICallbackSymbolTable reservedCallbacks,
        IUserFunctionSymbolSymbolTable userFunctions,
        IKspPreProcessorSymbolTable preProcessorSymbols,
        IPgsSymbolTable pgsSymbols )
    {
        Variables           = variables;
        UITypes             = uiTypes;
        Commands            = commands;
        UserCallbacks       = userCallbacks;
        ReservedCallbacks   = reservedCallbacks;
        UserFunctions       = userFunctions;
        PreProcessorSymbols = preProcessorSymbols;
        PgsSymbols          = pgsSymbols;
    }
}
