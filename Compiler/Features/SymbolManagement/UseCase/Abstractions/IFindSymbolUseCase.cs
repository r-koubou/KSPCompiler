using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;

public sealed record FindSymbolInput<TSymbol>(
    Predicate<TSymbol> Input
) where TSymbol : SymbolBase;

public sealed record FindSymbolOutput<TSymbol>(
    IReadOnlyCollection<TSymbol> Symbols
) where TSymbol : SymbolBase;

public interface IFindSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    Task<Result<FindSymbolOutput<TSymbol>, SymbolManagementFailureReason>> ExecuteAsync( FindSymbolInput<TSymbol> input, CancellationToken cancellationToken = default );
}
