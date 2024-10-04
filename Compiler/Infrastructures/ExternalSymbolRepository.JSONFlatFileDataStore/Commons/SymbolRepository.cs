using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JsonFlatFileDataStore;

using KSPCompiler.Commons;
using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons;

public abstract class SymbolRepository<TSymbol, TModel> : ISymbolRepository<TSymbol>
    where TSymbol : SymbolBase
    where TModel : class, ISymbolModel
{
    protected const string CommonKeyIdentifier = "repository_identifier";
    protected const string CommonKeyFormatVersion = "repository_format_version";

    protected FilePath RepositoryPath { get; }
    protected DataStore DataStore { get; }
    protected IDocumentCollection<TModel> Collection { get; }

    protected IDataTranslator<IEnumerable<TSymbol>, IReadOnlyCollection<TModel>> ToModelTranslator { get; }
    protected IDataTranslator<IEnumerable<TModel>, IReadOnlyCollection<TSymbol>> FromModelTranslator { get; }

    public int Count
        => Collection.Count;

    public SymbolRepository(
        FilePath repositoryPath,
        IDataTranslator<IEnumerable<TSymbol>, IReadOnlyCollection<TModel>> toModelTranslator,
        IDataTranslator<IEnumerable<TModel>, IReadOnlyCollection<TSymbol>> fromModelTranslator )
    {
        RepositoryPath      = repositoryPath;
        DataStore           = new DataStore( RepositoryPath.Path );
        Collection          = DataStore.GetCollection<TModel>( "symbols" );
        ToModelTranslator   = toModelTranslator;
        FromModelTranslator = fromModelTranslator;
    }

    protected void ValidateRepositoryIdentifier( string repositoryIdentifier )
    {
        string? identifier = null;

        try
        {
            identifier = DataStore.GetItem<string>( CommonKeyIdentifier );
        }
        catch
        {
            // シンボルデータが存在するが、リポジトリ識別子が存在しない
            if( Collection.Count > 0 )
            {
                throw new InvalidJsonRepositoryTypeException( $"Invalid repository type. (missing key: `{CommonKeyIdentifier}`)" );
            }
        }

        // リポジトリ識別子が一致しない（他の種類のシンボル管理ファイルの可能性、新規作成直後(identifier == null)は対象外）
        if( identifier != null && identifier != repositoryIdentifier )
        {
            throw new InvalidJsonRepositoryTypeException( $"Invalid repository type. (expected: {repositoryIdentifier}, actual: {identifier})" );
        }

        DataStore.ReplaceItem( CommonKeyIdentifier, repositoryIdentifier, upsert: true );
    }

    protected void ValidateRepositoryVersion( int expectedRepositoryVersion )
    {
        int actualRepositoryVersion = -1;

        try
        {
            actualRepositoryVersion = DataStore.GetItem<int>( CommonKeyFormatVersion );
        }
        catch
        {
            // ignore
        }

        if( actualRepositoryVersion != -1 && actualRepositoryVersion != expectedRepositoryVersion )
        {
            throw new InvalidJsonRepositoryVersionException( expectedRepositoryVersion, actualRepositoryVersion );
        }

        DataStore.ReplaceItem( CommonKeyFormatVersion, expectedRepositoryVersion, upsert: true );
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
    public virtual async Task<StoreResult> StoreAsync( TSymbol symbol, CancellationToken cancellationToken = default )
    {

        var model = ToModelTranslator.Translate( new[] { symbol } ).First();
        var existing = Collection.Find( x => x.Name == symbol.Name ).FirstOrDefault();

        var success = false;
        var createdCount = 0;
        var updatedCount = 0;
        var failedCount = 0;
        Exception? exception = null;

        try
        {
            if( existing != default )
            {
                model.UpdatedAt = DateTime.UtcNow;
                success        = await Collection.ReplaceOneAsync( existing.Id, model );

                if( success )
                {
                    updatedCount = 1;
                }
                else
                {
                    failedCount = 1;
                }
            }
            else
            {
                int newId = Collection.GetNextIdValue();
                if( newId < 0 )
                {
                    throw new InvalidOperationException( "Failed to get next id value. (overflow)" );
                }

                success = await Collection.InsertOneAsync( model );

                if( success )
                {
                    createdCount = 1;
                }
                else
                {
                    failedCount = 1;
                }
            }
        }
        catch( Exception e )
        {
            exception = e;
        }

        return new StoreResult( success, createdCount, updatedCount, failedCount, exception );
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<StoreResult> StoreAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var createdCount = 0;
        var updatedCount = 0;
        var failedCount = 0;

        foreach( var x in symbols )
        {
            var result = await StoreAsync( x, cancellationToken );
            if( !result.Success )
            {
                failedCount++;
            }
            else
            {
                createdCount += result.CreatedCount;
                updatedCount += result.UpdatedCount;
            }

            if( result.Exception == null )
            {
                continue;
            }

            return new StoreResult( true, createdCount, updatedCount, failedCount, result.Exception );
        }

        return new StoreResult( true, createdCount, updatedCount, failedCount );
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<DeleteResult> DeleteAsync( TSymbol symbol, CancellationToken cancellationToken = default )
    {
        var existing = Collection.Find( x => x.Name == symbol.Name ).FirstOrDefault();

        var success = false;
        var deletedCount = 0;
        var failedCount = 0;
        Exception? exception = null;

        try
        {
            if( existing != default )
            {
                var result = await Collection.DeleteOneAsync( existing.Id );
                if( result )
                {
                    deletedCount = 1;
                    success = true;
                }
                else
                {
                    failedCount = 1;
                }
            }
            else
            {
                return new DeleteResult( true, 0, 0 );
            }
        }
        catch( Exception e )
        {
            exception = e;
        }

        return new DeleteResult( success, deletedCount, failedCount, exception );
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<DeleteResult> DeleteAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var deletedCount = 0;
        var failedCount = 0;

        foreach( var x in symbols )
        {
            var result = await DeleteAsync( x, cancellationToken );
            if( !result.Success )
            {
                failedCount++;
            }
            else
            {
                deletedCount += result.DeletedCount;
            }

            if( result.Exception == null )
            {
                continue;
            }

            return new DeleteResult( true, deletedCount, failedCount, result.Exception );
        }

        return new DeleteResult( true, deletedCount, failedCount );
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<IReadOnlyCollection<TSymbol>> FindByNameAsync( string name, CancellationToken cancellationToken = default )
    {
        var founds = Collection.Find( x => name == x.Name );
        var result = FromModelTranslator.Translate( founds );

        await Task.CompletedTask;
        return result;
    }

    public virtual async Task<IReadOnlyCollection<TSymbol>> FindAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default )
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

        await Task.CompletedTask;
        return result;
    }

    public virtual async Task<IReadOnlyCollection<TSymbol>> FindAllAsync( CancellationToken cancellationToken = default )
    {
        var all = Collection.AsQueryable();

        await Task.CompletedTask;
        return FromModelTranslator.Translate( all );
    }
}
