using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

// ReSharper disable LocalizableElement

public interface ISymbolTemplateService
{
    public Task<ExportResult> ExportSymbolTemplateAsync( string exportFilePath, CancellationToken cancellationToken = default );
}
