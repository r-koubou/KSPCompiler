using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;

public sealed record  ExportSymbolTemplateInput<TSymbol>(
    ISymbolExporter<TSymbol> Input
) where TSymbol : SymbolBase;

public interface IExportSymbolTemplateUseCase<TSymbol> where TSymbol : SymbolBase
{
    Task<Result<Unit, SymbolManagementFailureReason>> ExecuteAsync( ExportSymbolTemplateInput<TSymbol> input, CancellationToken cancellationToken = default );
}
