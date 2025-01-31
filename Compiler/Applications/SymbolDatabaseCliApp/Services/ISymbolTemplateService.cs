using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.SymbolDatabaseControllers;

namespace KSPCompiler.Apps.SymbolDbManager.Services;

// ReSharper disable LocalizableElement

public interface ISymbolTemplateService
{
    public Task<ExportResult> ExportSymbolTemplateAsync( string exportFilePath, CancellationToken cancellationToken = default );
}
