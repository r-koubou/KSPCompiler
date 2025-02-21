using System;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class LoadBuiltinSymbolInputDataDetail(
    ISymbolRepository<VariableSymbol> variableSymbolRepository,
    ISymbolRepository<UITypeSymbol> uiTypeSymbolRepository,
    ISymbolRepository<CommandSymbol> commandSymbolRepository,
    ISymbolRepository<CallbackSymbol> callbackSymbolRepository )
{
    public ISymbolRepository<VariableSymbol> VariableSymbolRepository { get; } = variableSymbolRepository;
    public ISymbolRepository<UITypeSymbol> UITypeSymbolRepository { get; } = uiTypeSymbolRepository;
    public ISymbolRepository<CommandSymbol> CommandSymbolRepository { get; } = commandSymbolRepository;
    public ISymbolRepository<CallbackSymbol> CallbackSymbolRepository { get; } = callbackSymbolRepository;
}

public sealed class LoadBuiltinSymbolInputData( LoadBuiltinSymbolInputDataDetail inputData )
    : IInputPort<LoadBuiltinSymbolInputDataDetail>
{
    public LoadBuiltinSymbolInputDataDetail InputData { get; } = inputData;
}

public sealed class LoadBuiltinSymbolOutputData( AggregateSymbolTable outputData, bool result, Exception? error = null ) : IOutputPort
{
    public AggregateSymbolTable OutputData { get; } = outputData;
    public bool Result { get; } = result;
    public Exception? Error { get; } = error;
}

public interface ILoadBuiltinSymbolUseCase : IUseCase<LoadBuiltinSymbolInputData, LoadBuiltinSymbolOutputData>;
