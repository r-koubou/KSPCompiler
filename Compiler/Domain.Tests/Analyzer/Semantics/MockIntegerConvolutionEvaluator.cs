using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockIntegerConvolutionEvaluator : IIntegerConvolutionEvaluator
{
    private int? Result { get; }

    public MockIntegerConvolutionEvaluator() : this( 0 ) {}

    public MockIntegerConvolutionEvaluator( int? result )
    {
        Result = result;
    }

    public int? Evaluate( AstExpressionNode expr, int workingValueForRecursive )
        => Result;
}
