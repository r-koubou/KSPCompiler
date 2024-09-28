using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JsonFlatFileDataStore;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Translators;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;

public class CallbackSymbolRepository : ISymbolRepository<CallbackSymbol>
{
    private const string CurrentVersion = "20240929";

    private FilePath RepositoryPath { get; }
    private DataStore DataStore { get; }
    private IDocumentCollection<Symbol> Collection { get; }

    public int Count
        => Collection.Count;

    public CallbackSymbolRepository( FilePath repositoryPath )
    {
        RepositoryPath = repositoryPath;
        DataStore = new DataStore( RepositoryPath.Path );
        Collection = DataStore.GetCollection<Symbol>( "symbols" );

        DataStore.ReplaceItem( "version", CurrentVersion, upsert: true );
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
    public async Task<bool> StoreAsync( CallbackSymbol symbol, CancellationToken cancellationToken = default )
    {
        var jsonObject = new ToJsonObjectTranslator().Translate( new[] { symbol } );
        var existing = Collection.Find( x => x.Name == symbol.Name ).FirstOrDefault();

        if( existing != default )
        {
            var item = jsonObject.First();
            item.UpdatedAt = DateTime.UtcNow;
            return await Collection.ReplaceOneAsync( existing.Id, item );
        }
        else
        {
            return await Collection.InsertOneAsync( jsonObject.First() );
        }
    }

    ///
    /// <inheritdoc />
    ///
    public Task<bool> StoreAsync( IEnumerable<CallbackSymbol> symbols, CancellationToken cancellationToken = default )
        => throw new NotImplementedException();

    ///
    /// <inheritdoc />
    ///
    public Task<bool> DeleteAsync( CallbackSymbol symbol, CancellationToken cancellationToken = default )
        => throw new NotImplementedException();

    ///
    /// <inheritdoc />
    ///
    public Task<bool> DeleteAsync( IEnumerable<CallbackSymbol> symbols, CancellationToken cancellationToken = default )
        => throw new NotImplementedException();

    ///
    /// <inheritdoc />
    ///
    public Task<IEnumerable<CallbackSymbol>> FindAsync( Func<CallbackSymbol, bool> predicate, CancellationToken cancellationToken = default )
        => throw new NotImplementedException();
}
