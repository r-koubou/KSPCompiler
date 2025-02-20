using System.Collections.Concurrent;
using System.Collections.Generic;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.CoreNew.Compilation;

public sealed class CompilationCacheItem(
    ScriptLocation scriptLocation,
    IReadOnlyList<string>? allLinesText = null,
    AggregateSymbolTable? symbolTable = null,
    AstCompilationUnitNode? ast = null )
{
    public ScriptLocation ScriptLocation { get; } = scriptLocation;
    public IReadOnlyList<string> AllLinesText { get; } = allLinesText ?? [];

    public AggregateSymbolTable SymbolTable { get; } = symbolTable ?? new AggregateSymbolTable(
        builtInVariables: new VariableSymbolTable(),
        userVariables: new VariableSymbolTable(),
        uiTypes: new UITypeSymbolTable(),
        commands: new CommandSymbolTable(),
        builtInCallbacks: new CallbackSymbolTable(),
        userCallbacks: new CallbackSymbolTable(),
        userFunctions: new UserFunctionSymbolTable(),
        preProcessorSymbols: new PreProcessorSymbolTable()
    );

    public AstCompilationUnitNode Ast { get; } = ast ?? new AstCompilationUnitNode();
}

public sealed class CompilationCacheManager
{
    private readonly ConcurrentDictionary<ScriptLocation, CompilationCacheItem> symbolTableCache = new();

    public CompilationCacheItem GetCache( ScriptLocation scriptLocation )
    {
        return symbolTableCache.GetOrAdd(
            scriptLocation,
            new CompilationCacheItem( scriptLocation )
        );
    }

    public void UpdateCache( ScriptLocation scriptLocation, CompilationCacheItem newCacheItem )
    {
        symbolTableCache[ scriptLocation ] = newCacheItem;
    }

    public bool RemoveCache( ScriptLocation uri )
        => symbolTableCache.TryRemove( uri, out _ );
}
