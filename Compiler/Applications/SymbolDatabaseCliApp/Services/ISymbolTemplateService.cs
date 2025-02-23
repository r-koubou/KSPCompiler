using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Interactors.ApplicationServices.Symbols;

namespace KSPCompiler.Applications.SymbolDbManager.Services;

// ReSharper disable LocalizableElement

public interface ISymbolTemplateService
{
    public Task<ExportResult> ExportSymbolTemplateAsync( string exportFilePath, CancellationToken cancellationToken = default );
}
