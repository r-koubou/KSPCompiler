using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAppFramework;

using KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;
using KSPCompiler.Features.SymbolManagement.Gateways;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Commands;

// ReSharper disable LocalizableElement
public class DeleteCommand
{
    private static void HandleDeleteResult( DeleteResult result )
    {
        if( result.Success )
        {
            Console.WriteLine( "Delete success." );
            Console.WriteLine( $"Deleted: {result.DeletedCount}" );
        }
        else
        {
            Console.WriteLine( "Delete failed." );
            Console.WriteLine( result.Exception?.Message );
        }
    }

    /// <summary>
    /// Delete variables from specified database file.
    /// </summary>
    /// <param name="service">A service to delete variable symbols.</param>
    /// <param name="databaseFilePath">-d, A database file path to store.</param>
    /// <param name="deletePattern">-p, Delete symbol name pattern. Wildcard is supported.</param>
    /// <param name="cancellationToken"></param>
    [Command( "delete-variables" )]
    public async Task DeleteVariablesAsync( [FromServices] IVariableSymbolDatabaseService service, string databaseFilePath, string deletePattern = "*", CancellationToken cancellationToken = default )
    {
        var result = await service.DeleteSymbolsAsync( databaseFilePath, deletePattern, cancellationToken );
        HandleDeleteResult( result );
    }

    /// <summary>
    /// Delete commands from specified database file.
    /// </summary>
    /// <param name="service">A service to delete command symbols.</param>
    /// <param name="databaseFilePath">-d, A database file path to store.</param>
    /// <param name="deletePattern">-p, Delete symbol name pattern. Wildcard is supported.</param>
    /// <param name="cancellationToken"></param>
    [Command( "delete-commands" )]
    public async Task DeleteCommandsAsync( [FromServices] ICommandSymbolDatabaseService service, string databaseFilePath, string deletePattern = "*", CancellationToken cancellationToken = default )
    {
        var result = await service.DeleteSymbolsAsync( databaseFilePath, deletePattern, cancellationToken );
        HandleDeleteResult( result );
    }

    /// <summary>
    /// Delete callbacks from specified database file.
    /// </summary>
    /// <param name="service">A service to delete callback symbols.</param>
    /// <param name="databaseFilePath">-d, A database file path to store.</param>
    /// <param name="deletePattern">-p, Delete symbol name pattern. Wildcard is supported.</param>
    /// <param name="cancellationToken"></param>
    [Command( "delete-callbacks" )]
    public async Task DeleteCallbacksAsync( [FromServices] ICallbackSymbolDatabaseService service, string databaseFilePath, string deletePattern = "*", CancellationToken cancellationToken = default )
    {
        var result = await service.DeleteSymbolsAsync( databaseFilePath, deletePattern, cancellationToken );
        HandleDeleteResult( result );
    }

    /// <summary>
    /// Delete ui-types from specified database file.
    /// </summary>
    /// <param name="service">A service to delete ui-type symbols.</param>
    /// <param name="databaseFilePath">-d, A database file path to store.</param>
    /// <param name="deletePattern">-p, Delete symbol name pattern. Wildcard is supported.</param>
    /// <param name="cancellationToken"></param>
    [Command( "delete-ui-types" )]
    public async Task DeleteUITypeAsync( [FromServices] IUITypeSymbolDatabaseService service, string databaseFilePath, string deletePattern = "*", CancellationToken cancellationToken = default )
    {
        var result = await service.DeleteSymbolsAsync( databaseFilePath, deletePattern, cancellationToken );
        HandleDeleteResult( result );
    }
}
