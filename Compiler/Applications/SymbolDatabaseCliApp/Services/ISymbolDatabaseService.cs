using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.SymbolDatabaseControllers;

namespace KSPCompiler.Apps.SymbolDbManager.Services;

// ReSharper disable LocalizableElement

public partial interface ISymbolDatabaseService
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

public interface IVariableSymbolDatabaseService : ISymbolDatabaseService {}
public interface ICommandSymbolDatabaseService : ISymbolDatabaseService {}
public interface ICallbackSymbolDatabaseService : ISymbolDatabaseService {}
public interface IUITypeSymbolDatabaseService : ISymbolDatabaseService {}
