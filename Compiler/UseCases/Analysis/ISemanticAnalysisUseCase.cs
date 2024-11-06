using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Analysis;

public sealed class SemanticAnalysisInputDataDetail
{
    public ICompilerMessageManger MessageManager { get; }
    public AstCompilationUnitNode CompilationUnitNode { get; }
    public AggregateSymbolTable SymbolTable { get; }

    public SemanticAnalysisInputDataDetail(
        ICompilerMessageManger messageManager,
        AstCompilationUnitNode compilationUnitNode,
        AggregateSymbolTable symbolTable )
    {
        MessageManager      = messageManager;
        CompilationUnitNode = compilationUnitNode;
        SymbolTable         = symbolTable;
    }
}

public sealed class SemanticAnalysisInputData : IInputPort<SemanticAnalysisInputDataDetail>
{
    public SemanticAnalysisInputDataDetail InputData { get; }

    public SemanticAnalysisInputData( SemanticAnalysisInputDataDetail inputData )
    {
        InputData = inputData;
    }
}

public sealed class SemanticAnalysisOutputDataDetail
{
    public ICompilerMessageManger MessageManager { get; }
    public AstCompilationUnitNode CompilationUnitNode { get; }
    public AggregateSymbolTable SymbolTable { get; }

    public SemanticAnalysisOutputDataDetail(
        ICompilerMessageManger messageManager,
        AstCompilationUnitNode compilationUnitNode,
        AggregateSymbolTable symbolTable )
    {
        MessageManager      = messageManager;
        CompilationUnitNode = compilationUnitNode;
        SymbolTable         = symbolTable;
    }
}

public sealed class SemanticAnalysisOutputData : IOutputPort<SemanticAnalysisOutputDataDetail>
{
    public bool Result { get; }
    public Exception? Error { get; }
    public SemanticAnalysisOutputDataDetail OutputData { get; }

    public SemanticAnalysisOutputData( bool result, Exception? error, SemanticAnalysisOutputDataDetail outputData )
    {
        Result     = result;
        Error      = error;
        OutputData = outputData;
    }
}

public interface ISemanticAnalysisUseCase : IUseCase<SemanticAnalysisInputData, SemanticAnalysisOutputData> {}
