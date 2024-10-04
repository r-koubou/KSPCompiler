using System;
using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class FindSymbolInputData<TSymbol> : IInputPort<Predicate<TSymbol>> where TSymbol : SymbolBase
{
    public Predicate<TSymbol> InputData { get; }

    public FindSymbolInputData( Predicate<TSymbol> inputData )
    {
        InputData = inputData;
    }
}

public sealed class FindSymbolOutputData<TSymbol> : IOutputPort<IReadOnlyCollection<TSymbol>> where TSymbol : SymbolBase
{
    public bool Result { get; }
    public Exception? Error { get; }
    public IReadOnlyCollection<TSymbol> OutputData { get; }

    public FindSymbolOutputData( bool result, IReadOnlyCollection<TSymbol> outputData, Exception? error = null )
    {
        Result     = result;
        Error      = error;
        OutputData = outputData;
    }
}

public interface IFindSymbolUseCase<TSymbol> : IUseCase<FindSymbolInputData<TSymbol>, FindSymbolOutputData<TSymbol>> where TSymbol : SymbolBase {}
