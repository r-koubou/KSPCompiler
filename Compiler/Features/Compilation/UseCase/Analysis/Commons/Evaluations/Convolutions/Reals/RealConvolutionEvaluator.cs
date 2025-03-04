using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Convolutions.Reals;
using KSPCompiler.Shared.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations.Convolutions.Reals;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public sealed class RealConvolutionEvaluator : IRealConvolutionEvaluator
{
    private IRealConstantConvolutionCalculator ConstantCalculator { get; }
    private IRealBinaryOperatorConvolutionCalculator BinaryCalculator { get; }
    private IRealUnaryOperatorConvolutionCalculator UnaryCalculator { get; }

    public RealConvolutionEvaluator()
    {
        ConstantCalculator = new RealConstantConvolutionCalculator();
        BinaryCalculator   = new RealBinaryOperatorConvolutionCalculator( this );
        UnaryCalculator    = new RealUnaryOperatorConvolutionCalculator( this );
    }

    public double? Evaluate( IAstVisitor visitor, AstExpressionNode expr, double workingValueForRecursive )
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
