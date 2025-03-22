using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public abstract class SymbolRepository<TSymbol> : ISymbolRepository<TSymbol> where TSymbol : SymbolBase
{
    private readonly SemaphoreSlim semaphore = new( 1, 1 );

    protected Dictionary<Guid, TSymbol> Models { get; }
    protected ISymbolExporter<TSymbol>? RepositoryWriter { get; }

    private bool Disposed { get; set; }
    private bool Dirty { get; set; }
    public bool AutoFlush { get; set; }

    public int Count
        => Models.Count;

    protected SymbolRepository(
        ISymbolImporter<TSymbol>? repositoryReader = null,
        ISymbolExporter<TSymbol>? repositoryWriter = null,
        bool autoFlush = true )
    {
        RepositoryWriter  = repositoryWriter;
        Models    = repositoryReader == null ? [] : ImportImpl( repositoryReader );
        AutoFlush = autoFlush;
    }

    private Dictionary<Guid, TSymbol> ImportImpl( ISymbolImporter<TSymbol> importer )
    {
        var result = new Dictionary<Guid, TSymbol>();
        var symbols = importer.Import();

        foreach( var x in symbols )
        {
            result.Add( x.Id, x );
        }

        return result;
    }

    public List<TSymbol> ToList()
        => ToListAsync().GetAwaiter().GetResult();

    public async Task<List<TSymbol>> ToListAsync( CancellationToken cancellationToken = default )
    {
        await Task.CompletedTask;
        return Models.Values.OrderBy( x => x.Name.Value ).ToList();
    }

    public async Task FlushAsync()
    {
        if( Disposed )
        {
            throw new ObjectDisposedException( nameof( SymbolRepository<TSymbol> ) );
        }

        if( !Dirty )
        {
            return;
        }

        if( RepositoryWriter != null )
        {
            await RepositoryWriter.ExportAsync( ToList(), CancellationToken.None );
        }

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
            throw new ObjectDisposedException( nameof( SymbolRepository<TSymbol> ) );
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

    private async Task<StoreResult> StoreAsyncImpl( TSymbol symbol, CancellationToken _ = default )
    {
        if( !Models.TryGetValue( symbol.Id, out var existingSymbol ) )
        {
            symbol.CreatedAt = DateTime.UtcNow;
            symbol.UpdatedAt = DateTime.UtcNow;

            Models.Add( symbol.Id, symbol );
            Dirty = true;

            return new StoreResult(
                success: true,
                createdCount: 1,
                updatedCount: 0,
                failedCount: 0
            );
        }

        symbol.Id        = existingSymbol.Id;
        symbol.CreatedAt = existingSymbol.CreatedAt;
        symbol.UpdatedAt = DateTime.UtcNow;

        Models[ symbol.Id ] = symbol;
        Dirty               = true;

        await Task.CompletedTask;

        return new StoreResult(
            success: true,
            createdCount: 0,
            updatedCount: 1,
            failedCount: 0
        );
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<StoreResult> StoreAsync( TSymbol symbol, CancellationToken cancellationToken = default )
    {
        try
        {
            var result = await Lock( async () => await StoreAsyncImpl( symbol, cancellationToken ) );

            return await result;
        }
        catch( Exception e )
        {
            return new StoreResult(
                success: false,
                createdCount: 0,
                updatedCount: 0,
                failedCount: 1,
                exception: e
            );
        }
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<StoreResult> StoreAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var createdCount = 0;
        var updatedCount = 0;
        var failedCount = 0;

        try
        {
            await Lock( async () =>
                {
                    foreach( var x in symbols )
                    {
                        var result = await StoreAsyncImpl( x, cancellationToken );

                        createdCount += result.CreatedCount;
                        updatedCount += result.UpdatedCount;
                        failedCount  += result.FailedCount;
                    }
                }
            );

            return new StoreResult(
                success: true,
                createdCount: createdCount,
                updatedCount: updatedCount,
                failedCount: failedCount
            );
        }
        catch( Exception e )
        {
            return new StoreResult(
                success: false,
                createdCount: createdCount,
                updatedCount: updatedCount,
                failedCount: failedCount,
                exception: e
            );
        }
    }

    private async Task<DeleteResult> DeleteAsyncImpl( TSymbol symbol, CancellationToken _ = default )
    {
        if( !Models.TryGetValue( symbol.Id, out var existingSymbol ) )
        {
            return new DeleteResult();
        }

        Models.Remove( existingSymbol.Id );
        Dirty = true;

        await Task.CompletedTask;

        return new DeleteResult( deletedCount: 1 );
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<DeleteResult> DeleteAsync( TSymbol symbol, CancellationToken cancellationToken = default )
    {
        try
        {
            var result = await Lock( async () => await DeleteAsyncImpl( symbol, cancellationToken ) );

            return await result;
        }
        catch( Exception e )
        {
            return new DeleteResult(
                success: false,
                failedCount: 1,
                exception: e
            );
        }
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<DeleteResult> DeleteAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var deletedCount = 0;
        var failedCount = 0;

        try
        {
            await Lock( async () =>
                {
                    foreach( var x in symbols )
                    {
                        var result = await DeleteAsyncImpl( x, cancellationToken );

                        deletedCount += result.DeletedCount;
                        failedCount  += result.FailedCount;
                    }
                }
            );

            return new DeleteResult(
                success: true,
                deletedCount: deletedCount,
                failedCount: failedCount
            );
        }
        catch( Exception e )
        {
            return new DeleteResult(
                success: false,
                deletedCount: deletedCount,
                failedCount: failedCount,
                exception: e
            );
        }
    }

    ///
    /// <inheritdoc />
    ///
    public virtual async Task<IReadOnlyCollection<TSymbol>> FindByNameAsync( string name, CancellationToken cancellationToken = default )
        => await FindAsync( ( x ) => x.Name == name, cancellationToken );

    public virtual async Task<IReadOnlyCollection<TSymbol>> FindAsync( Predicate<TSymbol> predicate, CancellationToken cancellationToken = default )
    {
        var result = new List<TSymbol>();

        await Lock( async () =>
            {
                result.AddRange( Models.Values.Where( x => predicate( x ) ) );
                await Task.CompletedTask;
            }
        );

        return result;
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
