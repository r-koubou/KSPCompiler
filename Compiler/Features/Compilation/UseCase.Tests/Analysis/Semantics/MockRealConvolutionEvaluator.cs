using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Reals;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
