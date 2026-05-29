using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;

public sealed record DeleteSymbolInput<TSymbol>(
    Predicate<TSymbol> Input
) where TSymbol : SymbolBase;

public sealed record DeleteOutput(
    int DeletedCount,
    int FailedCount
);

public interface IDeleteSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    Task<Result<DeleteOutput, SymbolManagementFailureReason>> ExecuteAsync( DeleteSymbolInput<TSymbol> input, CancellationToken cancellationToken = default );
}
