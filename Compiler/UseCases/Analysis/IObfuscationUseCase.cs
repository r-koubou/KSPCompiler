using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Analysis;

public sealed class ObfuscationInputDataDetail
{
    public ICompilerMessageManger MessageManager { get; }
    public AstCompilationUnitNode CompilationUnitNode { get; }
    public AggregateSymbolTable SymbolTable { get; }

    public int DefaultOutputBufferCapacity { get; }

    public ObfuscationInputDataDetail(
        ICompilerMessageManger messageManager,
        AstCompilationUnitNode compilationUnitNode,
        AggregateSymbolTable symbolTable,
        int defaultOutputBufferCapacity = 16384 )
    {
        MessageManager              = messageManager;
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
