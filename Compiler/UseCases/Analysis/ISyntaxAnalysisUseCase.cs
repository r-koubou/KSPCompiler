using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Gateways.Parsers;

namespace KSPCompiler.UseCases.Analysis;

public class SyntaxAnalysisInputData : IInputPort<ISyntaxParser>
{
    public ISyntaxParser InputData { get; }

    public SyntaxAnalysisInputData( ISyntaxParser inputData )
    {
        InputData = inputData;
    }
}

public sealed class SyntaxAnalysisOutputData : IOutputPort<AstCompilationUnitNode>
{
    public bool Result { get; }

    public Exception? Error { get; }

    public AstCompilationUnitNode OutputData { get; }

    public SyntaxAnalysisOutputData( bool result, Exception? error, AstCompilationUnitNode outputData )
    {
        Result     = result;
        Error      = error;
        OutputData = outputData;
    }
}

public interface ISyntaxAnalysisUseCase : IUseCase<SyntaxAnalysisInputData, SyntaxAnalysisOutputData> {}
