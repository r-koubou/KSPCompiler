using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public sealed class RealConvolutionEvaluator : IConvolutionEvaluator<double>
{
    private IConvolutionOperandCalculator<double> OperandCalculator { get; }
    private IConvolutionBinaryCalculator<double> BinaryCalculator { get; }
    private IConvolutionUnaryCalculator<double> UnaryCalculator { get; }
    private IConvolutionConditionalEvaluator<double> ConditionalEvaluator { get; }

    public RealConvolutionEvaluator(
        IConvolutionOperandCalculator<double> operandCalculator,
        IConvolutionBinaryCalculator<double> binaryCalculator,
        IConvolutionUnaryCalculator<double> unaryCalculator,
        IConvolutionConditionalEvaluator<double> conditionalEvaluator )
    {
        OperandCalculator    = operandCalculator;
        BinaryCalculator     = binaryCalculator;
        UnaryCalculator      = unaryCalculator;
        ConditionalEvaluator = conditionalEvaluator;
    }

    public double? Evaluate( AstExpressionSyntaxNode expr, double workingValueForRecursive )
    {
        return expr.ChildNodeCount switch
        {
            0 => OperandCalculator.Calculate( expr, workingValueForRecursive ),
            1 => UnaryCalculator.Calculate( expr, workingValueForRecursive ),
            2 => BinaryCalculator.Calculate( expr, workingValueForRecursive ),
            _ => null
        };
    }
}
