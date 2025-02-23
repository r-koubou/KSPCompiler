using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAppFramework;

using KSPCompiler.Applications.SymbolDbManager.Services;
using KSPCompiler.Interactors.ApplicationServices.Symbols;

namespace KSPCompiler.Applications.SymbolDbManager.Commands;

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
    /// <param name="service">A service to export variables.</param>
    /// <param name="databaseFilePath">-d, A database file path to search symbols.</param>
    /// <param name="exportFilePath">-e, Export file path.</param>
    /// <param name="exportPattern">-p, Export symbol name pattern. Wildcard is supported.</param>
    /// <param name="cancellationToken"></param>
    [Command( "export-variables" )]
    public async Task ExportVariablesAsync( [FromServices] IVariableSymbolDatabaseService service, string databaseFilePath, string exportFilePath, string exportPattern = "*", CancellationToken cancellationToken = default )
    {
        var result = await service.ExportSymbolsAsync( databaseFilePath, exportFilePath, exportPattern, cancellationToken );
        HandleExportResult( result );
    }

    /// <summary>
    /// Export commands to specified file.
    /// </summary>
    /// <param name="service">A service to export commands.</param>
    /// <param name="databaseFilePath">-d, A database file path to search symbols.</param>
    /// <param name="exportFilePath">-e, Export file path.</param>
    /// <param name="exportPattern">-p, Export symbol name pattern. Wildcard is supported.</param>
    /// <param name="cancellationToken"></param>
    [Command( "export-commands" )]
    public async Task ExportCommandsAsync( [FromServices] ICommandSymbolDatabaseService service, string databaseFilePath, string exportFilePath, string exportPattern = "*", CancellationToken cancellationToken = default )
    {
        var result = await service.ExportSymbolsAsync( databaseFilePath, exportFilePath, exportPattern, cancellationToken );
        HandleExportResult( result );
    }

    /// <summary>
    /// Export callbacks to specified file.
    /// </summary>
    /// <param name="service">A service to export callbacks.</param>
    /// <param name="databaseFilePath">-d, A database file path to search symbols.</param>
    /// <param name="exportFilePath">-e, Export file path.</param>
    /// <param name="exportPattern">-p, Export symbol name pattern. Wildcard is supported.</param>
    /// <param name="cancellationToken"></param>
    [Command( "export-callbacks" )]
    public async Task ExportCallbacksAsync( [FromServices] ICallbackSymbolDatabaseService service, string databaseFilePath, string exportFilePath, string exportPattern = "*", CancellationToken cancellationToken = default )
    {
        var result = await service.ExportSymbolsAsync( databaseFilePath, exportFilePath, exportPattern, cancellationToken );
        HandleExportResult( result );
    }

    /// <summary>
    /// Export ui-types to specified file.
    /// </summary>
    /// <param name="service">A service to export ui-types.</param>
    /// <param name="databaseFilePath">-d, A database file path to search symbols.</param>
    /// <param name="exportFilePath">-e, Export file path.</param>
    /// <param name="exportPattern">-p, Export symbol name pattern. Wildcard is supported.</param>
    /// <param name="cancellationToken"></param>
    [Command( "export-ui-types" )]
    public async Task ExportUITypeAsync( [FromServices] IUITypeSymbolDatabaseService service, string databaseFilePath, string exportFilePath, string exportPattern = "*", CancellationToken cancellationToken = default )
    {
        var result = await service.ExportSymbolsAsync( databaseFilePath, exportFilePath, exportPattern, cancellationToken );
        HandleExportResult( result );
    }
}
