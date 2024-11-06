using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.UseCases.Analysis;

public sealed class SemanticAnalysisInputDataDetail
{
    public ICompilerMessageManger MessageManager { get; }
    public AstCompilationUnitNode CompilationUnitNode { get; }

    public SemanticAnalysisInputDataDetail( ICompilerMessageManger messageManager, AstCompilationUnitNode compilationUnitNode )
    {
        MessageManager      = messageManager;
        CompilationUnitNode = compilationUnitNode;
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

public sealed class SemanticAnalysisOutputData : IOutputPort<AstCompilationUnitNode>
{
    public bool Result { get; }
    public Exception? Error { get; }
    public AstCompilationUnitNode OutputData { get; }

    public SemanticAnalysisOutputData( bool result, Exception? error, AstCompilationUnitNode outputData )
    {
        Result     = result;
        Error      = error;
        OutputData = outputData;
    }
}

public interface ISemanticAnalysisUseCase : IUseCase<SemanticAnalysisInputData, SemanticAnalysisOutputData> {}
