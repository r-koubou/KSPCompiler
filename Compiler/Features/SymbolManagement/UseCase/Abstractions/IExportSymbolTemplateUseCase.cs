using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;

public sealed record  ExportSymbolTemplateInputPort<TSymbol>(
    ISymbolExporter<TSymbol> Input
) where TSymbol : SymbolBase;

public interface IExportSymbolTemplateUseCase<TSymbol> where TSymbol : SymbolBase
{
    Task<Result<Unit, SymbolManagementFailureReason>> ExecuteAsync( ExportSymbolTemplateInputPort<TSymbol> input, CancellationToken cancellationToken = default );
}
