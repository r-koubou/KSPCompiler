using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.UseCases.Analysis;

public sealed class PreprocessInputDataDetail
{
    public ICompilerMessageManger MessageManager { get; }
    public AstCompilationUnitNode CompilationUnitNode { get; }

    public PreprocessInputDataDetail( ICompilerMessageManger messageManager, AstCompilationUnitNode compilationUnitNode )
    {
        MessageManager      = messageManager;
        CompilationUnitNode = compilationUnitNode;
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

public sealed class PreprocessOutputData : IOutputPort<AstCompilationUnitNode>
{
    public bool Result { get; }
    public Exception? Error { get; }
    public AstCompilationUnitNode OutputData { get; }

    public PreprocessOutputData( bool result, Exception? error, AstCompilationUnitNode outputData )
    {
        Result     = result;
        Error      = error;
        OutputData = outputData;
    }
}

public interface IPreprocessUseCase : IUseCase<PreprocessInputData, PreprocessOutputData> {}
