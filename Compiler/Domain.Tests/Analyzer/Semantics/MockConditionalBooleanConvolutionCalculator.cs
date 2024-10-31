using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockConditionalBooleanConvolutionCalculator : IConditionalConvolutionCalculator
{
    private bool? Result { get; }

    public MockConditionalBooleanConvolutionCalculator() : this( true ) {}

    public MockConditionalBooleanConvolutionCalculator( bool? result )
    {
        Result = result;
    }

    public bool? Calculate( AstExpressionNode expr )
        => Result;
}
