using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Strings;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockStringConvolutionEvaluator : IStringConvolutionEvaluator
{
    private string? Result { get; }

    public MockStringConvolutionEvaluator() : this( "abc" ) {}

    public MockStringConvolutionEvaluator( string? result )
    {
        Result = result;
    }

    public string? Evaluate( IAstVisitor visitor, AstExpressionNode expr, string workingValueForRecursive )
        => Result;
}
