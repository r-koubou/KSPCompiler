using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public interface IConvolutionEvaluator<T> where T : struct
{
    /// <summary>
    /// Evaluate expression node
    /// </summary>
    /// <param name="expr">Expression node</param>
    /// <param name="workingValueForRecursive">Working value for recursive evaluation. First call should be 0.</param>
    /// <returns>Evaluated value. If constant value is not found, returns null.</returns>
    T? Evaluate( AstExpressionSyntaxNode expr, T workingValueForRecursive );
}
