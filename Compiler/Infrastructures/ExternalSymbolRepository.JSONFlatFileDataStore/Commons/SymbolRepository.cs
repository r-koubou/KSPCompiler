using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JsonFlatFileDataStore;

using KSPCompiler.Commons;
using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons;

public abstract class SymbolRepository<TSymbol, TModel> : ISymbolRepository<TSymbol>
    where TSymbol : SymbolBase
    where TModel : class, IModelBase
{
    protected FilePath RepositoryPath { get; }
    protected DataStore DataStore { get; }
    protected IDocumentCollection<TModel> Collection { get; }

    protected IDataTranslator<IEnumerable<TSymbol>, IEnumerable<TModel>> ToModelTranslator { get; }
    protected IDataTranslator<IEnumerable<TModel>, IEnumerable<TSymbol>> FromModelTranslator { get; }

    public int Count
        => Collection.Count;

    public SymbolRepository(
        FilePath repositoryPath,
        IDataTranslator<IEnumerable<TSymbol>, IEnumerable<TModel>> toModelTranslator,
        IDataTranslator<IEnumerable<TModel>, IEnumerable<TSymbol>> fromModelTranslator )
    {
        RepositoryPath      = repositoryPath;
        DataStore           = new DataStore( RepositoryPath.Path );
        Collection          = DataStore.GetCollection<TModel>( "symbols" );
        ToModelTranslator   = toModelTranslator;
        FromModelTranslator = fromModelTranslator;
    }

    ///
    /// <inheritdoc />
    ///
    public void Dispose()
    {
        try
        {
            DataStore.Dispose();
        }
        catch( Exception )
        {
            // ignored
        }
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<bool> StoreAsync( TSymbol symbol, CancellationToken cancellationToken = default )
    {
        var jsonObject = ToModelTranslator.Translate( new[] { symbol } );
        var existing = Collection.Find( x => x.Name == symbol.Name ).FirstOrDefault();

        if( existing != default )
        {
            var item = jsonObject.First();
            item.UpdatedAt = DateTime.UtcNow;
            return await Collection.ReplaceOneAsync( existing.Id, item );
        }

        return await Collection.InsertOneAsync( jsonObject.First() );
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<bool> StoreAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        foreach( var x in symbols )
        {
            if( !await StoreAsync( x, cancellationToken ) )
            {
                return false;
            }
        }

        return true;
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<bool> DeleteAsync( TSymbol symbol, CancellationToken cancellationToken = default )
    {
        var existing = Collection.Find( x => x.Name == symbol.Name ).FirstOrDefault();

        if( existing != default )
        {
            return await Collection.DeleteOneAsync( existing.Id );
        }

        return true;
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<bool> DeleteAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        foreach( var x in symbols )
        {
            if( !await DeleteAsync( x, cancellationToken ) )
            {
                return false;
            }
        }

        return true;
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<IEnumerable<TSymbol>> FindByNameAsync( string name, CancellationToken cancellationToken = default )
    {
        var founds = Collection.Find( x => name == x.Name );
        var result = FromModelTranslator.Translate( founds );

        return result;
    }

    public virtual async Task<IEnumerable<TSymbol>> FindAsync( Func<TSymbol, bool> predicate, CancellationToken cancellationToken = default )
    {
        var result = new List<TSymbol>();
        var all = Collection.AsQueryable();

        foreach( var x in all )
        {
            var symbol = FromModelTranslator.Translate( new[] { x } ).First();
            if( predicate( symbol ) )
            {
                result.Add( symbol );
            }
        }

        return result;
    }

    public virtual async Task<IEnumerable<TSymbol>> FindAllAsync( CancellationToken cancellationToken = default )
    {
        var all = Collection.AsQueryable();

        return FromModelTranslator.Translate( all );
    }
}
