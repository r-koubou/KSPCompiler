using System;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class LoadBuiltinSymbolInputData(
    AggregateSymbolRepository inputData
) : InputPort<AggregateSymbolRepository>( inputData );

public sealed class LoadBuiltinSymbolOutputData(
    AggregateSymbolTable outputData,
    bool result,
    Exception? error = null
) : OutputPort<AggregateSymbolTable>( outputData, result, error );

public interface ILoadBuiltinSymbolUseCase
    : IUseCase<LoadBuiltinSymbolInputData, LoadBuiltinSymbolOutputData>;
