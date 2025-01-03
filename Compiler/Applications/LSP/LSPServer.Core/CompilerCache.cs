using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol;

namespace KSPCompiler.LSPServer.Core;

public sealed class CompilerCache
{
    private readonly Dictionary<DocumentUri, AggregateSymbolTable> symbolTableCache = new();

    public AggregateSymbolTable GetSymbolTable( DocumentUri uri )
    {
        if( symbolTableCache.TryGetValue( uri, out var symbolTable ) )
        {
            return symbolTable;
        }

        symbolTable = new AggregateSymbolTable(
            builtInVariables: new VariableSymbolTable(),
            userVariables: new VariableSymbolTable(),
            uiTypes: new UITypeSymbolTable(),
            commands: new CommandSymbolTable(),
            builtInCallbacks: new CallbackSymbolTable(),
            userCallbacks: new CallbackSymbolTable(),
            userFunctions: new UserFunctionSymbolTable(),
            preProcessorSymbols: new PreProcessorSymbolTable()
        );

        symbolTableCache.Add( uri, symbolTable );

        return symbolTable;
    }

    public void SetSymbolTable( DocumentUri uri, AggregateSymbolTable symbolTable )
    {
        if( symbolTableCache.TryAdd( uri, symbolTable ) )
        {
            return;
        }

        symbolTableCache[ uri ] = symbolTable;
    }

    public bool RemoveSymbolTable( DocumentUri uri )
        => symbolTableCache.Remove( uri );
}
