using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Shared.IO.LocalStorages;
using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables;
using KSPCompiler.Features.SymbolManagement.UseCase.ApplicationServices;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Path;
using KSPCompiler.SymbolManagement.Repository.Yaml.Variables;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

// ReSharper disable LocalizableElement
public class VariableSymbolDatabaseService : IVariableSymbolDatabaseService
{
    public async Task<ImportResult> ImportSymbolsAsync( string databaseFilePath, string importFilePath, CancellationToken cancellationToken = default )
    {
        try
        {
            var importPath = new FilePath( importFilePath );
            var repositoryPath = new FilePath( databaseFilePath );
            var reader = new LocalTextContentReader( importPath );
            var importer = new TsvVariableSymbolImporter( reader );
            using var repository = new VariableSymbolRepository( repositoryPath );
            var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

            return await applicationService.ImportAsync( importer, cancellationToken );
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
            var exportPath = new FilePath( exportFilePath );
            var repositoryPath = new FilePath( databaseFilePath );
            var writer = new LocalTextContentWriter( exportPath );
            using var repository = new VariableSymbolRepository( repositoryPath );
            var exporter = new TsvVariableSymbolExporter( writer );
            var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

            return await applicationService.ExportAsync(
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
            var repositoryPath = new FilePath( databaseFilePath );
            using var repository = new VariableSymbolRepository( repositoryPath );
            var applicationService = new SymbolDatabaseApplicationService<VariableSymbol>( repository );

            return await applicationService.DeleteAsync(
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
