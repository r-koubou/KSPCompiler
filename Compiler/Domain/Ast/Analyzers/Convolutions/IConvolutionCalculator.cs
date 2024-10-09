using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Base interface for convolution calculators
/// </summary>
/// <typeparam name="T">Calculator return type</typeparam>
public interface IConvolutionCalculator<T> where T : struct
{
    /// <summary>
    /// Calculate the value of the expression
    /// </summary>
    /// <param name="expr">Expression node</param>
    /// <param name="workingValueForRecursive">Working value for recursive evaluation. First call should be default value of `T`.</param>
    /// <returns>Calculated value. If constant value is not found, returns null.</returns>
    T? Calculate( AstExpressionSyntaxNode expr, T workingValueForRecursive );
}
