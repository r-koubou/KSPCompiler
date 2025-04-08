using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Integers;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

public class MockIntegerConvolutionEvaluator : IIntegerConvolutionEvaluator
{
    private int? Result { get; }

    public MockIntegerConvolutionEvaluator() : this( 0 ) {}

    public MockIntegerConvolutionEvaluator( int? result )
    {
        Result = result;
    }

    public int? Evaluate( IAstVisitor visitor, AstExpressionNode expr, int workingValueForRecursive )
        => Result;
}
