using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Booleans;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

public class MockBooleanConvolutionEvaluator : IBooleanConvolutionEvaluator
{
    private bool? Result { get; }

    public MockBooleanConvolutionEvaluator() : this( false ) {}

    public MockBooleanConvolutionEvaluator( bool? result )
    {
        Result = result;
    }

    public bool? Evaluate( IAstVisitor visitor, AstExpressionNode expr, bool workingValueForRecursive )
        => Result;
}
