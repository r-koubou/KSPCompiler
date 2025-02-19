using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Interactors.ApplicationServices.Symbol;

namespace KSPCompiler.Applications.SymbolDbManager.Services;

// ReSharper disable LocalizableElement

public interface ISymbolDatabaseService
{
    public Task<ImportResult> ImportSymbolsAsync( string databaseFilePath, string importFilePath, CancellationToken cancellationToken = default );
    public Task<ExportResult> ExportSymbolsAsync( string databaseFilePath, string exportFilePath, string exportPattern, CancellationToken cancellationToken = default );
    public Task<DeleteResult> DeleteSymbolsAsync( string databaseFilePath, string deletePattern, CancellationToken cancellationToken = default );

    public static Regex WildCardToRegexPattern( string wildcardPattern )
    {
        return new Regex(
            "^" + Regex.Escape( wildcardPattern ).Replace( @"\*", ".*" ).Replace( @"\?", "." ) + "$",
            RegexOptions.Singleline
        );
    }
}

public interface ICallbackSymbolDatabaseService : ISymbolDatabaseService;
public interface ICommandSymbolDatabaseService : ISymbolDatabaseService;
public interface IVariableSymbolDatabaseService : ISymbolDatabaseService;
public interface IUITypeSymbolDatabaseService : ISymbolDatabaseService;
