using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions;

/// <summary>
/// Calculator for convolution operations with conditional operations for struct (primitive) types
/// </summary>
/// <typeparam name="T">Configured conditional expression type (int/real etc.)</typeparam>
public interface IPrimitiveConvolutionConditionalEvaluator<T> where T : struct
{
    /// <summary>
    /// Calculate conditional expression
    /// </summary>
    /// <param name="expr">Expression node</param>
    /// <param name="abortTraverseToken">Token for aborting traversal</param>
    /// <returns>Evaluated value. If constant value is not found, returns null.</returns>
    bool? Evaluate( AstExpressionNode expr, AbortTraverseToken abortTraverseToken );
}

public sealed class NullConvolutionConditionalEvaluator<T> : IPrimitiveConvolutionCalculator<bool> where T : struct
{
    public bool? Calculate( AstExpressionNode expr, bool workingValueForRecursive )
        => null;
}
