using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Gateways.Symbols;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Path;

using YamlDotNet.Serialization;

namespace KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml;

public abstract class SymbolRepository<TSymbol, TRootModel, TModel> : ISymbolRepository<TSymbol>
    where TSymbol : SymbolBase
    where TRootModel : ISymbolRootModel<TModel>, new()
    where TModel : ISymbolModel
{
    private readonly SemaphoreSlim semaphore = new( 1, 1 );

    protected FilePath RepositoryPath { get; }
    protected List<TModel> Models { get; }
    protected IDataTranslator<TModel, TSymbol> FromModelTranslator { get; }

    private bool Disposed { get; set; }
    private bool Dirty { get; set; }

    public bool AutoFlush { get; set; }

    public int Count
        => Models.Count;

    [Obsolete( "For temporary use only." )]
    public List<TModel> All
        => [..Models];

    protected SymbolRepository(
        FilePath repositoryPath,
        IDataTranslator<TModel, TSymbol> fromModelTranslator,
        bool autoFlush = true )
    {
        RepositoryPath      = repositoryPath;
        FromModelTranslator = fromModelTranslator;
        Models              = Load();
        AutoFlush           = autoFlush;
    }

    private List<TModel> Load()
    {
        if( !RepositoryPath.Exists )
        {
            return [];
        }

        var yamlText = File.ReadAllText( RepositoryPath.Path );
        var root = new DeserializerBuilder()
                  .Build()
                  .Deserialize<TRootModel>( yamlText );

        if( root == null )
        {
            throw new InvalidOperationException( "Could not load models from repository." );
        }

        return root.Data;
    }

    public async Task FlushAsync()
    {
        if( !Dirty )
        {
            return;
        }

        var sorted = Models.OrderBy( x => x.Name ).ToList();
        var root = new TRootModel
        {
            Data = sorted
        };
        var yamlText = new SerializerBuilder()
                      .Build()
                      .Serialize( root );

        await File.WriteAllTextAsync( RepositoryPath.Path, yamlText );

        Dirty = false;
    }

    public void Flush()
        => FlushAsync().GetAwaiter().GetResult();

    ///
    /// <inheritdoc />
    ///
    public void Dispose()
    {
        if( Disposed )
        {
            throw new ObjectDisposedException( nameof( SymbolRepository<TSymbol, TRootModel, TModel> ) );
        }

        try
        {
            if( AutoFlush )
            {
                Flush();
            }
        }
        finally
        {
            Disposed = true;
        }
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<IReadOnlyCollection<TSymbol>> FindByNameAsync( string name, CancellationToken cancellationToken = default )
        => await FindAsync( ( x ) => x.Name == name, cancellationToken );

    public virtual async Task<IReadOnlyCollection<TSymbol>> FindAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default )
    {
        return await Lock(
            () => new List<TSymbol>(
                Models
                   .Select( FromModelTranslator.Translate )
                   .Where( x => predicate( x ) )
            )
        );
    }

    public virtual async Task<IReadOnlyCollection<TSymbol>> FindAllAsync( CancellationToken cancellationToken = default )
    {
        return await Lock(
            () => new List<TSymbol>(
                Models.Select( FromModelTranslator.Translate )
            )
        );
    }

    private async Task<T> Lock<T>( Func<T> func )
    {
        await semaphore.WaitAsync();

        try
        {
            return func();
        }
        finally
        {
            semaphore.Release();
        }
    }
}
