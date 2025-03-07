using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Strings;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
