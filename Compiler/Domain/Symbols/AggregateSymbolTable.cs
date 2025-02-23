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

    public static void Merge( AggregateSymbolTable source, AggregateSymbolTable target, bool clearTarget = false )
    {
        if( clearTarget )
        {
            target.Clear();
        }

        // Variables
        // UI Type
        // Command
        // User Function
        // PreProcessor Symbol
        source.BuiltInVariables.AddRange( target.BuiltInVariables );
        source.UserVariables.AddRange( target.UserVariables );
        source.UITypes.AddRange( target.UITypes );
        source.Commands.AddRange( target.Commands );
        source.UserFunctions.AddRange( target.UserFunctions );
        source.PreProcessorSymbols.AddRange( target.PreProcessorSymbols );

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
