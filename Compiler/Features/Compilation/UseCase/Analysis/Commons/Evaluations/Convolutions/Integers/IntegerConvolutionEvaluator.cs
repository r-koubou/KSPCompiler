using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Integers;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Integers;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public sealed class IntegerConvolutionEvaluator : IIntegerConvolutionEvaluator
{
    private IIntegerConstantConvolutionCalculator ConstantCalculator { get; }
    private IIntegerBinaryOperatorConvolutionCalculator BinaryCalculator { get; }
    private IIntegerUnaryOperatorConvolutionCalculator UnaryCalculator { get; }

    public IntegerConvolutionEvaluator()
    {
        ConstantCalculator = new IntegerConstantConvolutionCalculator();
        BinaryCalculator   = new IntegerBinaryOperatorConvolutionCalculator( this );
        UnaryCalculator    = new IntegerUnaryOperatorConvolutionCalculator( this );
    }

    public int? Evaluate( IAstVisitor visitor, AstExpressionNode expr, int workingValueForRecursive )
    {
        return expr.ChildNodeCount switch
        {
            0 => ConstantCalculator.Calculate( visitor, expr, workingValueForRecursive ),
            1 => UnaryCalculator.Calculate( visitor, expr, workingValueForRecursive ),
            2 => BinaryCalculator.Calculate( visitor, expr, workingValueForRecursive ),
            _ => null
        };
    }
}
