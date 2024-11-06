using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Conditions;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

public class MockConditionalConvolutionEvaluator : IConditionalConvolutionEvaluator
{
    private bool? Result { get; }

    public MockConditionalConvolutionEvaluator() : this( false ) {}

    public MockConditionalConvolutionEvaluator( bool? result )
    {
        Result = result;
    }

    public bool? Evaluate( IAstVisitor visitor, AstExpressionNode expr, bool workingValueForRecursive )
        => Result;
}
