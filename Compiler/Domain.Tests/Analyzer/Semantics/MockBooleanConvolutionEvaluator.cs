using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockBooleanConvolutionEvaluator : IBooleanConvolutionEvaluator
{
    private bool? Result { get; }

    public MockBooleanConvolutionEvaluator() : this( false ) {}

    public MockBooleanConvolutionEvaluator( bool? result )
    {
        Result = result;
    }

    public bool? Evaluate( AstExpressionNode expr, bool workingValueForRecursive )
        => Result;
}
