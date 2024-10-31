using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions;

/// <summary>
/// Calculator for convolution operations with conditional operations
/// </summary>
public interface IConditionalConvolutionCalculator
{
    /// <summary>
    /// Calculate conditional expression
    /// </summary>
    /// <param name="expr">Expression node</param>
    /// <returns>Evaluated value. If constant value is not found, returns null.</returns>
    bool? Calculate( AstExpressionNode expr );
}
