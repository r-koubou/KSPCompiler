using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Gateways;

namespace KSPCompiler.UseCases.Analysis;

public class SyntaxAnalysisInputData( ISyntaxParser inputData ) : IInputPort<ISyntaxParser>
{
    public ISyntaxParser InputData { get; } = inputData;
}

public sealed class SyntaxAnalysisOutputData( bool result, Exception? error, AstCompilationUnitNode outputData )
    : IOutputPort<AstCompilationUnitNode>
{
    public bool Result { get; } = result;
    public Exception? Error { get; } = error;
    public AstCompilationUnitNode OutputData { get; } = outputData;
}

public interface ISyntaxAnalysisUseCase : IUseCase<SyntaxAnalysisInputData, SyntaxAnalysisOutputData> {}
