using System.Collections.Concurrent;
using System.Collections.Generic;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol;

namespace KSPCompiler.LSPServer.Core.Compilations;

public sealed class CompilerCacheItem
{
    public IReadOnlyList<string> AllLinesText { get; }
    public AggregateSymbolTable SymbolTable { get; }
    public AstCompilationUnitNode Ast { get; }

    public CompilerCacheItem(
        IReadOnlyList<string>? allLinesText = null,
        AggregateSymbolTable? symbolTable = null,
        AstCompilationUnitNode? ast = null )
    {
        AllLinesText = allLinesText ?? [];
        SymbolTable = symbolTable ?? new AggregateSymbolTable(
            builtInVariables: new VariableSymbolTable(),
            userVariables: new VariableSymbolTable(),
            uiTypes: new UITypeSymbolTable(),
            commands: new CommandSymbolTable(),
            builtInCallbacks: new CallbackSymbolTable(),
            userCallbacks: new CallbackSymbolTable(),
            userFunctions: new UserFunctionSymbolTable(),
            preProcessorSymbols: new PreProcessorSymbolTable()
        );
        Ast = ast ?? new AstCompilationUnitNode();
    }
}

public sealed class CompilerCacheService
{
    private readonly ConcurrentDictionary<DocumentUri, CompilerCacheItem> symbolTableCache = new();

    public CompilerCacheItem GetCache( DocumentUri uri )
    {
        return symbolTableCache.GetOrAdd(
            uri,
            new CompilerCacheItem()
        );
    }

    public void UpdateCache( DocumentUri uri, CompilerCacheItem newCacheItem )
    {
        symbolTableCache[ uri ] = newCacheItem;
    }

    public bool RemoveCache( DocumentUri uri )
        => symbolTableCache.TryRemove( uri, out _ );
}
