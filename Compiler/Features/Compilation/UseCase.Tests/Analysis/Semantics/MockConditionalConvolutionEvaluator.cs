using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Conditions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

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
