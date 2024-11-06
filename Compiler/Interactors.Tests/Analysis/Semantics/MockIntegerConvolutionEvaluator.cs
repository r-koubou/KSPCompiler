using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Integers;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockIntegerConvolutionEvaluator : IIntegerConvolutionEvaluator
{
    private int? Result { get; }

    public MockIntegerConvolutionEvaluator() : this( 0 ) {}

    public MockIntegerConvolutionEvaluator( int? result )
    {
        Result = result;
    }

    public int? Evaluate( IAstVisitor visitor, AstExpressionNode expr, int workingValueForRecursive )
        => Result;
}
