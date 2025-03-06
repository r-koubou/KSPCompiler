using System.Collections.Generic;

using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;

public interface ICompilationCacheManager
{
    bool ContainsCache( ScriptLocation scriptLocation );
    CompilationCacheItem GetCache( ScriptLocation scriptLocation );
    void UpdateCache( ScriptLocation scriptLocation, CompilationCacheItem newCacheItem );
    bool RemoveCache( ScriptLocation uri );
}

public sealed class CompilationCacheItem(
    ScriptLocation scriptLocation,
    IReadOnlyList<string>? allLinesText = null,
    AggregateSymbolTable? symbolTable = null,
    AstCompilationUnitNode? ast = null )
{
    public ScriptLocation ScriptLocation { get; } = scriptLocation;
    public IReadOnlyList<string> AllLinesText { get; } = allLinesText ?? [ ];
    public AggregateSymbolTable SymbolTable { get; } = symbolTable ?? AggregateSymbolTable.Default();
    public AstCompilationUnitNode Ast { get; } = ast ?? new AstCompilationUnitNode();
}
