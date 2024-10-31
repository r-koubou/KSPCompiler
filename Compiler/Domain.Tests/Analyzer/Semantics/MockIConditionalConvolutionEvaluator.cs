using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockIConditionalConvolutionEvaluator : IConditionalConvolutionEvaluator
{
    private bool? Result { get; }

    public MockIConditionalConvolutionEvaluator() : this( true ) {}

    public MockIConditionalConvolutionEvaluator( bool? result )
    {
        Result = result;
    }

    public bool? Evaluate( AstExpressionNode expr, bool workingValueForRecursive )
        => Result;
}
