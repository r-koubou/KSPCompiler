using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables;
using KSPCompiler.Infrastructures.Commons.LocalStorages;
using KSPCompiler.SymbolDatabaseControllers;

using DeleteResult = KSPCompiler.SymbolDatabaseControllers.DeleteResult;

namespace KSPCompiler.Apps.SymbolDbManager.Services;

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
            var controller = new SymbolDatabaseController<VariableSymbol>( repository );

            return await controller.ImportAsync( importer, cancellationToken );
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
            var controller = new SymbolDatabaseController<VariableSymbol>( repository );

            return await controller.ExportAsync(
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
            var controller = new SymbolDatabaseController<VariableSymbol>( repository );

            return await controller.DeleteAsync(
                symbol => regexPattern.IsMatch( symbol.Name.Value ),
                cancellationToken
            );
        }
        catch( Exception e )
        {
            return new DeleteResult( false, 0, e );
        }
    }
}
