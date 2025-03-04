using System;

using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

public sealed class ObfuscationInputDataDetail
{
    public IEventEmitter EventEmitter { get; }
    public AstCompilationUnitNode CompilationUnitNode { get; }
    public AggregateSymbolTable SymbolTable { get; }

    public int DefaultOutputBufferCapacity { get; }

    public ObfuscationInputDataDetail(
        IEventEmitter eventEmitter,
        AstCompilationUnitNode compilationUnitNode,
        AggregateSymbolTable symbolTable,
        int defaultOutputBufferCapacity = 16384 )
    {
        EventEmitter              = eventEmitter;
        CompilationUnitNode         = compilationUnitNode;
        SymbolTable                 = symbolTable;
        DefaultOutputBufferCapacity = defaultOutputBufferCapacity;
    }
}

public sealed class ObfuscationInputData(
    ObfuscationInputDataDetail inputInput
) : InputPort<ObfuscationInputDataDetail>( inputInput );

public sealed class ObfuscationOutputData(
    string outputData,
    bool result,
    Exception? error = null
) : OutputPort<string>( outputData, result, error );

public interface IObfuscationUseCase
    : IUseCase<ObfuscationInputData, ObfuscationOutputData> {}
