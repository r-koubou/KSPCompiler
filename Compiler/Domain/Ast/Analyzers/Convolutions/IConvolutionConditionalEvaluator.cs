using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with conditional operations
/// </summary>
/// <typeparam name="T">Configured conditional expression type (int/real etc.)</typeparam>
public interface IConvolutionConditionalEvaluator<T> where T : struct
{
    /// <summary>
    /// Calculate conditional expression
    /// </summary>
    /// <param name="expr">Expression node</param>
    /// <param name="abortTraverseToken">Token for aborting traversal</param>
    /// <returns>Evaluated value. If constant value is not found, returns null.</returns>
    bool? Evaluate( AstExpressionSyntaxNode expr, AbortTraverseToken abortTraverseToken );
}

public sealed class NullConvolutionConditionalEvaluator<T> : IConvolutionCalculator<bool> where T : struct
{
    public bool? Calculate( AstExpressionSyntaxNode expr, bool workingValueForRecursive )
        => null;
}
