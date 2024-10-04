using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAppFramework;

using KSPCompiler.Apps.SymbolDbManager.Services;
using KSPCompiler.SymbolDatabaseControllers;

namespace KSPCompiler.Apps.SymbolDbManager.Commands;

// ReSharper disable LocalizableElement
public class ExportCommand
{
    private static void HandleExportResult( ExportResult result )
    {
        if( result.Success )
        {
            Console.WriteLine( "Export success." );
        }
        else
        {
            Console.WriteLine( "Export failed." );
            Console.WriteLine( result.Exception?.Message );
        }
    }

    /// <summary>
    /// Export variables to specified file.
    /// </summary>
    /// <param name="service">A service to import variables.</param>
    /// <param name="databaseFilePath">-d, A database file path to store.</param>
    /// <param name="exportFilePath">-e, Export file path.</param>
    /// <param name="exportPattern">-p, Export symbol name pattern. Wildcard is supported.</param>
    /// <param name="cancellationToken"></param>
    [Command( "export-variables" )]
    public async Task ExportVariablesAsync( [FromServices] IVariableSymbolDatabaseService service, string databaseFilePath, string exportFilePath, string exportPattern = "*", CancellationToken cancellationToken = default )
    {
        var result = await service.ExportSymbolsAsync( databaseFilePath, exportFilePath, exportPattern, cancellationToken );
        HandleExportResult( result );
    }
}
