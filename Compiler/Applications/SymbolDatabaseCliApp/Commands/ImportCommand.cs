using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAppFramework;

using KSPCompiler.Apps.SymbolDbManager.Services;
using KSPCompiler.SymbolDatabaseControllers;

namespace KSPCompiler.Apps.SymbolDbManager.Commands;

// ReSharper disable LocalizableElement
public class ImportCommand
{
    private static void HandleImportResult( ImportResult result )
    {
        if( result.Success )
        {
            Console.WriteLine( "Import success." );
            Console.WriteLine( $"Created: {result.Created}, Updated: {result.Updated}, Failed: {result.Failed}" );
        }
        else
        {
            Console.WriteLine( "Import failed." );
            Console.WriteLine( result.Exception?.Message );
        }
    }

    /// <summary>
    /// Import variables to specified database file.
    /// </summary>
    /// <param name="service">A service to import variables.</param>
    /// <param name="databaseFilePath">-d, A database file path to store.</param>
    /// <param name="importFilePath">-i, A importing file path.</param>
    /// <param name="cancellationToken"></param>
    [Command( "import-variables" )]
    public async Task ImportVariablesAsync( [FromServices] IVariableSymbolDatabaseService service, string databaseFilePath, string importFilePath, CancellationToken cancellationToken = default )
    {
        var result = await service.ImportSymbolsAsync( databaseFilePath, importFilePath, cancellationToken );
        HandleImportResult( result );
    }

    /// <summary>
    /// Import commands to specified database file.
    /// </summary>
    /// <param name="service">A service to import commands.</param>
    /// <param name="databaseFilePath">-d, A database file path to store.</param>
    /// <param name="importFilePath">-i, A importing file path.</param>
    /// <param name="cancellationToken"></param>
    [Command( "import-commands" )]
    public async Task ImportCommandsAsync( [FromServices] ICommandSymbolDatabaseService service, string databaseFilePath, string importFilePath, CancellationToken cancellationToken = default )
    {
        var result = await service.ImportSymbolsAsync( databaseFilePath, importFilePath, cancellationToken );
        HandleImportResult( result );
    }
}
