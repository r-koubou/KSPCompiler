using System;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class LoadBuiltinSymbolInputData( AggregateSymbolRepository inputData )
    : IInputPort<AggregateSymbolRepository>
{
    public AggregateSymbolRepository InputData { get; } = inputData;
}

public sealed class LoadBuiltinSymbolOutputData( AggregateSymbolTable outputData, bool result, Exception? error = null ) : IOutputPort
{
    public AggregateSymbolTable OutputData { get; } = outputData;
    public bool Result { get; } = result;
    public Exception? Error { get; } = error;
}

public interface ILoadBuiltinSymbolUseCase : IUseCase<LoadBuiltinSymbolInputData, LoadBuiltinSymbolOutputData>;
