using System.Collections.Concurrent;

using KSPCompiler.UseCases.LanguageServer;
using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.Interactors.LanguageServer.Compilation;

public sealed class CompilationCacheManager : ICompilationCacheManager
{
    private readonly ConcurrentDictionary<ScriptLocation, CompilationCacheItem> symbolTableCache = new();

    public bool ContainsCache( ScriptLocation scriptLocation )
        => symbolTableCache.ContainsKey( scriptLocation );

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
