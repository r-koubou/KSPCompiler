using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.ApplicationServices;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.IO.Local;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables;
using KSPCompiler.Shared.IO.Symbols.Yaml.Variables;
using KSPCompiler.SymbolManagement.Repository.Yaml;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

// ReSharper disable LocalizableElement
public class VariableSymbolDatabaseService(
    IEventEmitter? eventEmitter = null
) : IVariableSymbolDatabaseService
{
    public async Task<ImportResult> ImportSymbolsAsync( string databaseFilePath, string importFilePath, CancellationToken cancellationToken = default )
    {
        try
        {
            var repositoryReader = new YamlVariableSymbolImporter( new LocalTextContentReader( databaseFilePath ) );
            var repositoryWriter = new YamlVariableSymbolExporter( new LocalTextContentWriter( databaseFilePath ) );
            var importer = new TsvVariableSymbolImporter( new LocalTextContentReader( importFilePath ) );

            using var repository = new VariableSymbolRepository(
                repositoryImporter: repositoryReader,
                repositoryExporter: repositoryWriter,
                eventEmitter: eventEmitter
            );

            var service = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

            return await service.ImportAsync( importer, cancellationToken );
        }
        catch( Exception e )
        {
            return new ImportResult( false, 0, 0, 0, e );
        }
    }

    public async Task<ExportResult> ExportSymbolsAsync( string databaseFilePath, string exportFilePath, string exportPattern, CancellationToken cancellationToken = default )
    {
        try
        {
            var regexPattern = ISymbolDatabaseService.WildCardToRegexPattern( exportPattern );

            var repositoryReader = new YamlVariableSymbolImporter( new LocalTextContentReader( databaseFilePath ) );
            var repositoryWriter = new YamlVariableSymbolExporter( new LocalTextContentWriter( databaseFilePath ) );
            var exporter = new TsvVariableSymbolExporter( new LocalTextContentWriter( exportFilePath ) );

            using var repository = new VariableSymbolRepository(
                repositoryImporter: repositoryReader,
                repositoryExporter: repositoryWriter,
                eventEmitter: eventEmitter
            );

            var service = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

            return await service.ExportAsync(
                exporter,
                symbol => regexPattern.IsMatch( symbol.Name.Value ),
                cancellationToken
            );
        }
        catch( Exception e )
        {
            return new ExportResult( false, e );
        }
    }

    public async Task<DeleteResult> DeleteSymbolsAsync( string databaseFilePath, string deletePattern, CancellationToken cancellationToken = default )
    {
        try
        {
            var regexPattern = ISymbolDatabaseService.WildCardToRegexPattern( deletePattern );

            var repositoryReader = new YamlVariableSymbolImporter( new LocalTextContentReader( databaseFilePath ) );
            var repositoryWriter = new YamlVariableSymbolExporter( new LocalTextContentWriter( databaseFilePath ) );

            using var repository = new VariableSymbolRepository(
                repositoryImporter: repositoryReader,
                repositoryExporter: repositoryWriter,
                eventEmitter: eventEmitter
            );

            var service = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

            return await service.DeleteAsync(
                symbol => regexPattern.IsMatch( symbol.Name.Value ),
                cancellationToken
            );
        }
        catch( Exception e )
        {
            return new DeleteResult(
                success: false,
                deletedCount: 0,
                failedCount: 0,
                exception: e
            );
        }
    }
}
