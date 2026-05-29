using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;

public sealed record ImportSymbolInput<TSymbol>(
    ISymbolImporter<TSymbol> Importer
) where TSymbol : SymbolBase;

public sealed record ImportSymbolOutput(
    int CreatedCount,
    int UpdatedCount,
    int FailedCount
);

public interface IImportSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    Task<Result<ImportSymbolOutput, SymbolManagementFailureReason>> ExecuteAsync( ImportSymbolInput<TSymbol> input, CancellationToken cancellationToken = default );
}
