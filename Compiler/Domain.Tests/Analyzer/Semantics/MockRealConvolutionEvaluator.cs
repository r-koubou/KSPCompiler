using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockRealConvolutionEvaluator : IRealConvolutionEvaluator
{
    public double? Evaluate( AstExpressionNode expr, double workingValueForRecursive )
        => 0.0;
}
