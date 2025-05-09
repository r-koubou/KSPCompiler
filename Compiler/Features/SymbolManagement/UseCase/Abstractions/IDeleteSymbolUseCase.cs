using System;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;

public sealed class DeleteSymbolInputData<TSymbol>(
    Predicate<TSymbol> inputInput
) : InputPort<Predicate<TSymbol>>( inputInput ) where TSymbol : SymbolBase;

public sealed class DeleteOutputData(
    DeleteOutputDetail outputData,
    bool result,
    Exception? error = null
) : OutputPort<DeleteOutputDetail>( outputData, result, error );

public sealed class DeleteOutputDetail( int deletedCount, int failedCount )
{
    public int DeletedCount { get; } = deletedCount;
    public int FailedCount { get; } = failedCount;
}

public interface IDeleteSymbolUseCase<TSymbol>
    : IUseCase<DeleteSymbolInputData<TSymbol>, DeleteOutputData>
    where TSymbol : SymbolBase {}
