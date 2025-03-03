namespace KSPCompiler.Features.Compilation.Domain.Symbols;

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

    public static AggregateSymbolTable Default()
        => new(
            builtInVariables: new VariableSymbolTable(),
            userVariables: new VariableSymbolTable(),
            uiTypes: new UITypeSymbolTable(),
            commands: new CommandSymbolTable(),
            builtInCallbacks: new CallbackSymbolTable(),
            userCallbacks: new CallbackSymbolTable(),
            userFunctions: new UserFunctionSymbolTable(),
            preProcessorSymbols: new PreProcessorSymbolTable()
        );

    public void Clear()
    {
        BuiltInVariables.Clear();
        UserVariables.Clear();
        UITypes.Clear();
        Commands.Clear();
        UserCallbacks.Clear();
        BuiltInCallbacks.Clear();
        UserFunctions.Clear();
        PreProcessorSymbols.Clear();
    }

    public static void Merge( AggregateSymbolTable source, AggregateSymbolTable target )
    {
        // Variables
        // UI Type
        // Command
        // User Function
        // PreProcessor Symbol
        target.BuiltInVariables.AddRange( source.BuiltInVariables );
        target.UserVariables.AddRange( source.UserVariables );
        target.UITypes.AddRange( source.UITypes );
        target.Commands.AddRange( source.Commands );
        target.UserFunctions.AddRange( source.UserFunctions );

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
