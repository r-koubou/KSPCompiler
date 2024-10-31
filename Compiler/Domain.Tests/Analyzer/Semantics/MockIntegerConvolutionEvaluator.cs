using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockIntegerConvolutionEvaluator : IIntegerConvolutionEvaluator
{
    public int? Evaluate( AstExpressionNode expr, int workingValueForRecursive )
        => 0;
}
