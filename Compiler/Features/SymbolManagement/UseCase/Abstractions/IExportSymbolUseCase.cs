using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;

public sealed record ExportSymbolInputData<TSymbol>(
    ISymbolExporter<TSymbol> Exporter,
    Predicate<TSymbol> Predicate
) where TSymbol : SymbolBase;

public interface IExportSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    Task<Result<Unit, SymbolManagementFailureReason>> ExecuteAsync( ExportSymbolInputData<TSymbol> input, CancellationToken cancellationToken = default );
}
