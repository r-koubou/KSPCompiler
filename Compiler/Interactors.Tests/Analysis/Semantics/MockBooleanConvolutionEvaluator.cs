using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Booleans;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

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
