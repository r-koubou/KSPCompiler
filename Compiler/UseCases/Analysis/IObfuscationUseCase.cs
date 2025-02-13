using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.EventEmitting;

namespace KSPCompiler.UseCases.Analysis;

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

public sealed class ObfuscationInputData : IInputPort<ObfuscationInputDataDetail>
{
    public ObfuscationInputDataDetail InputData { get; }

    public ObfuscationInputData( ObfuscationInputDataDetail inputData )
    {
        InputData = inputData;
    }
}

public sealed class ObfuscationOutputData : IOutputPort<string>
{
    public bool Result { get; }
    public Exception? Error { get; }
    public string OutputData { get; }

    public ObfuscationOutputData( bool result, Exception? error, string outputData )
    {
        Result     = result;
        Error      = error;
        OutputData = outputData;
    }
}

public interface IObfuscationUseCase : IUseCase<ObfuscationInputData, ObfuscationOutputData> {}
