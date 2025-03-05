using System;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public sealed class LoadBuiltinSymbolInputData(
    AggregateSymbolRepository inputInput
) : InputPort<AggregateSymbolRepository>( inputInput );

public sealed class LoadBuiltinSymbolOutputData(
    AggregateSymbolTable outputData,
    bool result,
    Exception? error = null
) : OutputPort<AggregateSymbolTable>( outputData, result, error );

public interface ILoadBuiltinSymbolUseCase
    : IUseCase<LoadBuiltinSymbolInputData, LoadBuiltinSymbolOutputData>;
