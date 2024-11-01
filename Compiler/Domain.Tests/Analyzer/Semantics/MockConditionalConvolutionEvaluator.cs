using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockConditionalConvolutionEvaluator : IConditionalConvolutionEvaluator
{
    private bool? Result { get; }

    public MockConditionalConvolutionEvaluator() : this( false ) {}

    public MockConditionalConvolutionEvaluator( bool? result )
    {
        Result = result;
    }

    public bool? Evaluate( AstExpressionNode expr, bool workingValueForRecursive )
        => Result;
}
