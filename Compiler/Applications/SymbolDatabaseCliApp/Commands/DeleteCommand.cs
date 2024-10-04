using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAppFramework;

using KSPCompiler.Apps.SymbolDbManager.Services;
using KSPCompiler.SymbolDatabaseControllers;

namespace KSPCompiler.Apps.SymbolDbManager.Commands;

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
    /// <param name="service">A service to import variables.</param>
    /// <param name="databaseFilePath">-d, A database file path to store.</param>
    /// <param name="deletePattern">-p, Delete symbol name pattern. Wildcard is supported.</param>
    /// <param name="cancellationToken"></param>
    [Command( "delete-variables" )]
    public async Task DeleteVariablesAsync( [FromServices] IVariableSymbolDatabaseService service, string databaseFilePath, string deletePattern = "*", CancellationToken cancellationToken = default )
    {
        var result = await service.DeleteSymbolsAsync( databaseFilePath, deletePattern, cancellationToken );
        HandleDeleteResult( result );
    }
}
