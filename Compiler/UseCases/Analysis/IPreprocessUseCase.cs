using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Analysis;

public sealed class PreprocessInputDataDetail
{
    public ICompilerMessageManger MessageManager { get; }
    public AstCompilationUnitNode CompilationUnitNode { get; }
    public AggregateSymbolTable SymbolTable { get; }

    public PreprocessInputDataDetail(
        ICompilerMessageManger messageManager,
        AstCompilationUnitNode compilationUnitNode,
        AggregateSymbolTable symbolTable )
    {
        MessageManager      = messageManager;
        CompilationUnitNode = compilationUnitNode;
        SymbolTable         = symbolTable;
    }
}

public sealed class PreprocessInputData : IInputPort<PreprocessInputDataDetail>
{
    public PreprocessInputDataDetail InputData { get; }

    public PreprocessInputData( PreprocessInputDataDetail inputData )
    {
        InputData = inputData;
    }
}

public sealed class PreprocessOutputDataDetail
{
    public ICompilerMessageManger MessageManager { get; }
    public AstCompilationUnitNode CompilationUnitNode { get; }
    public AggregateSymbolTable SymbolTable { get; }

    public PreprocessOutputDataDetail(
        ICompilerMessageManger messageManager,
        AstCompilationUnitNode compilationUnitNode,
        AggregateSymbolTable symbolTable )
    {
        MessageManager      = messageManager;
        CompilationUnitNode = compilationUnitNode;
        SymbolTable         = symbolTable;
    }
}

public sealed class PreprocessOutputData : IOutputPort<PreprocessOutputDataDetail>
{
    public bool Result { get; }
    public Exception? Error { get; }
    public PreprocessOutputDataDetail OutputData { get; }

    public PreprocessOutputData( bool result, Exception? error, PreprocessOutputDataDetail outputData )
    {
        Result     = result;
        Error      = error;
        OutputData = outputData;
    }
}

public interface IPreprocessUseCase : IUseCase<PreprocessInputData, PreprocessOutputData> {}
