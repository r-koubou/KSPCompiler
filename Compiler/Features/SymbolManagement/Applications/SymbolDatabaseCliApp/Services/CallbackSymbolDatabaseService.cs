using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.ApplicationServices;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.IO.Local;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks;
using KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks;
using KSPCompiler.SymbolManagement.Repository.Yaml;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

// ReSharper disable LocalizableElement
public class CallbackSymbolDatabaseService(
    IEventEmitter? eventEmitter = null
) : ICallbackSymbolDatabaseService
{
    public async Task<ImportResult> ImportSymbolsAsync( string databaseFilePath, string importFilePath, CancellationToken cancellationToken = default )
    {
        try
        {
            var repositoryReader = new YamlCallbackSymbolImporter( new LocalTextContentReader( databaseFilePath ) );
            var repositoryWriter = new YamlCallbackSymbolExporter( new LocalTextContentWriter( databaseFilePath ) );
            var importer = new TsvCallbackSymbolImporter( new LocalTextContentReader( importFilePath ) );

            using var repository = new CallbackSymbolRepository(
                repositoryImporter: repositoryReader,
                repositoryExporter: repositoryWriter,
                eventEmitter: eventEmitter
            );

            var service = new SymbolDatabaseApplicationService<CallbackSymbol>( repository );

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

            var repositoryReader = new YamlCallbackSymbolImporter( new LocalTextContentReader( databaseFilePath ) );
            var repositoryWriter = new YamlCallbackSymbolExporter( new LocalTextContentWriter( databaseFilePath ) );
            var exporter = new TsvCallbackSymbolExporter( new LocalTextContentWriter( exportFilePath ) );

            using var repository = new CallbackSymbolRepository(
                repositoryImporter: repositoryReader,
                repositoryExporter: repositoryWriter,
                eventEmitter: eventEmitter
            );

            var service = new SymbolDatabaseApplicationService<CallbackSymbol>( repository );

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

            var repositoryReader = new YamlCallbackSymbolImporter( new LocalTextContentReader( databaseFilePath ) );
            var repositoryWriter = new YamlCallbackSymbolExporter( new LocalTextContentWriter( databaseFilePath ) );

            using var repository = new CallbackSymbolRepository(
                repositoryImporter: repositoryReader,
                repositoryExporter: repositoryWriter,
                eventEmitter: eventEmitter
            );

            var service = new SymbolDatabaseApplicationService<CallbackSymbol>( repository );

            return await service.DeleteAsync(
                symbol => regexPattern.IsMatch( symbol.Name.Value ),
                cancellationToken
            );
        }
        catch( Exception e )
        {
            return new DeleteResult( success: false, exception: e );
        }
    }
}
