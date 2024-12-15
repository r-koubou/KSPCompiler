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
            new VariableSymbolTable(),
            new UITypeSymbolTable(),
            new CommandSymbolTable(),
            new CallbackSymbolTable(),
            new CallbackSymbolTable(),
            new UserFunctionSymbolTable(),
            new PreProcessorSymbolTable()
        );

        symbolTableCache.Add( uri, symbolTable );

        return symbolTable;
    }
}
