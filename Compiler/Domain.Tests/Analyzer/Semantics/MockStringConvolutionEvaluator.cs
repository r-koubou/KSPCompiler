using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockStringConvolutionEvaluator : IStringConvolutionEvaluator
{
    private string? Result { get; }

    public MockStringConvolutionEvaluator() : this( "abc" ) {}

    public MockStringConvolutionEvaluator( string? result )
    {
        Result = result;
    }

    public string? Evaluate( AstExpressionNode expr, string workingValueForRecursive )
        => Result;
}
