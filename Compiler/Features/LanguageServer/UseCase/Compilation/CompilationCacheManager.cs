using System.Collections.Concurrent;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;

namespace KSPCompiler.Features.LanguageServer.UseCase.Compilation;

public sealed class CompilationCacheManager : ICompilationCacheManager
{
    private readonly ConcurrentDictionary<string, CompilationCacheItem> symbolTableCache = new();

    private static string GetCacheKey( ScriptLocation scriptLocation )
        => scriptLocation.ScriptUri.ToString();

    public bool ContainsCache( ScriptLocation scriptLocation )
        => symbolTableCache.ContainsKey( GetCacheKey( scriptLocation ) );

    public CompilationCacheItem GetCache( ScriptLocation scriptLocation )
    {
        return symbolTableCache.GetOrAdd(
            GetCacheKey( scriptLocation ),
            new CompilationCacheItem( scriptLocation )
        );
    }

    public void UpdateCache( ScriptLocation scriptLocation, CompilationCacheItem newCacheItem )
    {
        symbolTableCache[ GetCacheKey( scriptLocation ) ] = newCacheItem;
    }

    public bool RemoveCache( ScriptLocation scriptLocation )
        => symbolTableCache.TryRemove( GetCacheKey( scriptLocation ), out _ );
}
