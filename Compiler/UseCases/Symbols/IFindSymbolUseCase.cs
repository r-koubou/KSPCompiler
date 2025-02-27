using System;
using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class FindSymbolInputData<TSymbol>(
    Predicate<TSymbol> inputInput
) : InputPort<Predicate<TSymbol>>( inputInput ) where TSymbol : SymbolBase;

public sealed class FindSymbolOutputData<TSymbol>(
    IReadOnlyCollection<TSymbol> outputData,
    bool result,
    Exception? error = null
) : OutputPort<IReadOnlyCollection<TSymbol>>( outputData, result, error ) where TSymbol : SymbolBase;

public interface IFindSymbolUseCase<TSymbol>
    : IUseCase<FindSymbolInputData<TSymbol>, FindSymbolOutputData<TSymbol>>
    where TSymbol : SymbolBase;
