using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAppFramework;

using KSPCompiler.Apps.SymbolDbManager.Services;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.SymbolDatabaseControllers;

namespace KSPCompiler.Apps.SymbolDbManager.Commands;

// ReSharper disable LocalizableElement
public class TemplateCommand
{
    private static void HandleExportResult( ExportResult result )
    {
        if( result.Success )
        {
            Console.WriteLine( "Generate template success." );
        }
        else
        {
            Console.WriteLine( "Generate template failed." );
            Console.WriteLine( result.Exception?.Message );
        }
    }

    /// <summary>
    /// Export variables template for database importing format.
    /// </summary>
    /// <param name="service">A service to export variables.</param>
    /// <param name="exportFilePath">-e, Export file path.</param>
    /// <param name="cancellationToken"></param>
    [Command( "generate-template-variables" )]
    public async Task ExportVariablesTemplateAsync( [FromServices] SymbolTemplateService<VariableSymbol> service, string exportFilePath, CancellationToken cancellationToken = default )
    {
        var result = await service.ExportSymbolTemplateAsync( exportFilePath, cancellationToken );
        HandleExportResult( result );
    }

    /// <summary>
    /// Export commands template for database importing format.
    /// </summary>
    /// <param name="service">A service to export commands.</param>
    /// <param name="exportFilePath">-e, Export file path.</param>
    /// <param name="cancellationToken"></param>
    [Command( "generate-template-commands" )]
    public async Task ExportCommandsTemplateAsync( [FromServices] SymbolTemplateService<CommandSymbol> service, string exportFilePath, CancellationToken cancellationToken = default )
    {
        var result = await service.ExportSymbolTemplateAsync( exportFilePath, cancellationToken );
        HandleExportResult( result );
    }

    /// <summary>
    /// Export callbacks template for database importing format.
    /// </summary>
    /// <param name="service">A service to export callback.</param>
    /// <param name="exportFilePath">-e, Export file path.</param>
    /// <param name="cancellationToken"></param>
    [Command( "generate-template-callbacks" )]
    public async Task ExportCallbacksTemplateAsync( [FromServices] SymbolTemplateService<CallbackSymbol> service, string exportFilePath, CancellationToken cancellationToken = default )
    {
        var result = await service.ExportSymbolTemplateAsync( exportFilePath, cancellationToken );
        HandleExportResult( result );
    }

    /// <summary>
    /// Export ui types template for database importing format.
    /// </summary>
    /// <param name="service">A service to export variables.</param>
    /// <param name="exportFilePath">-e, Export file path.</param>
    /// <param name="cancellationToken"></param>
    [Command( "generate-template-ui-types" )]
    public async Task ExportUITypesTemplateAsync( [FromServices] SymbolTemplateService<UITypeSymbol> service, string exportFilePath, CancellationToken cancellationToken = default )
    {
        var result = await service.ExportSymbolTemplateAsync( exportFilePath, cancellationToken );
        HandleExportResult( result );
    }
}
