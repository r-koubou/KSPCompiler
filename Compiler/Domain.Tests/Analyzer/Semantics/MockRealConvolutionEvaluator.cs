using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockRealConvolutionEvaluator : IRealConvolutionEvaluator
{
    private double? Result { get; }

    public MockRealConvolutionEvaluator() : this( 0.0 ) {}

    public MockRealConvolutionEvaluator( double? result )
    {
        Result = result;
    }

    public double? Evaluate( IAstVisitor visitor, AstExpressionNode expr, double workingValueForRecursive )
        => Result;
}
