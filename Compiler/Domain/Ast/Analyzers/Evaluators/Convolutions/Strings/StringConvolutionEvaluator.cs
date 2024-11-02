using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;

/// <summary>
/// Evaluating string convolution expressions
/// </summary>
public sealed class StringConvolutionEvaluator : IStringConvolutionEvaluator
{
    private IStringConstantConvolutionCalculator ConstantCalculator { get; }
    private IStringConcatenateOperatorConvolutionCalculator ConcatenateCalculator { get; }

    public StringConvolutionEvaluator()
    {
        ConstantCalculator    = new StringConstantConvolutionCalculator();
        ConcatenateCalculator = new StringConcatenateOperatorConvolutionCalculator( this );
    }

    public string? Evaluate( IAstVisitor visitor, AstExpressionNode expr, string workingValueForRecursive )
    {
        return expr.ChildNodeCount switch
        {
            0 => ConstantCalculator.Calculate( visitor, expr, workingValueForRecursive ),
            2 => ConcatenateCalculator.Calculate( visitor, expr, workingValueForRecursive ),
            _ => null
        };
    }
}
