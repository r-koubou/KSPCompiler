using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.Core.Extensions;

public static class AggregateSymbolTableExtension
{
    public static AggregateSymbolTable CreateBuiltInSymbolsOnly( this AggregateSymbolTable baseSymbolTable )
    {
        return new AggregateSymbolTable(
            builtInVariables: baseSymbolTable.BuiltInVariables,
            userVariables: new VariableSymbolTable(),
            uiTypes: baseSymbolTable.UITypes,
            commands: baseSymbolTable.Commands,
            builtInCallbacks: baseSymbolTable.BuiltInCallbacks,
            userCallbacks: new CallbackSymbolTable(),
            userFunctions: new UserFunctionSymbolTable(),
            preProcessorSymbols: new PreProcessorSymbolTable()
        );
    }
}
