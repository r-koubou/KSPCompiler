using System.Collections.Concurrent;
using System.Collections.Generic;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol;

namespace KSPCompiler.Applications.LSPServer.Core.Compilations;

public sealed class CompilerCacheItem
{
    public DocumentUri Uri { get; }
    public IReadOnlyList<string> AllLinesText { get; }
    public AggregateSymbolTable SymbolTable { get; }
    public AstCompilationUnitNode Ast { get; }

    public CompilerCacheItem(
        DocumentUri uri,
        IReadOnlyList<string>? allLinesText = null,
        AggregateSymbolTable? symbolTable = null,
        AstCompilationUnitNode? ast = null )
    {
        AllLinesText = allLinesText ?? [];
        SymbolTable  = symbolTable ?? AggregateSymbolTable.Default();
        Ast          = ast ?? new AstCompilationUnitNode();
    }
}

public sealed class CompilerCacheService
{
    private readonly ConcurrentDictionary<DocumentUri, CompilerCacheItem> symbolTableCache = new();

    public CompilerCacheItem GetCache( DocumentUri uri )
    {
        return symbolTableCache.GetOrAdd(
            uri,
            new CompilerCacheItem( uri )
        );
    }

    public void UpdateCache( DocumentUri uri, CompilerCacheItem newCacheItem )
    {
        symbolTableCache[ uri ] = newCacheItem;
    }

    public bool RemoveCache( DocumentUri uri )
        => symbolTableCache.TryRemove( uri, out _ );
}
