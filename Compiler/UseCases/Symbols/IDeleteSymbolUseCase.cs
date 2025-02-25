using System;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class DeleteSymbolInputData<TSymbol>(
    Predicate<TSymbol> inputData
) : InputPort<Predicate<TSymbol>>( inputData ) where TSymbol : SymbolBase;

public sealed class DeleteOutputData(
    DeleteOutputDetail outputData,
    bool result,
    Exception? error = null
) : OutputPort<DeleteOutputDetail>( outputData, result, error );

public sealed class DeleteOutputDetail( int deletedCount )
{
    public int DeletedCount { get; } = deletedCount;
}

public interface IDeleteSymbolUseCase<TSymbol>
    : IUseCase<DeleteSymbolInputData<TSymbol>, DeleteOutputData>
    where TSymbol : SymbolBase {}
