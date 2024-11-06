using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Reals;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

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
