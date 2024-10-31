using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockConditionalConvolutionCalculator : IConditionalConvolutionCalculator
{
    public bool? Calculate( AstExpressionNode expr )
        => throw new System.NotImplementedException();
}
