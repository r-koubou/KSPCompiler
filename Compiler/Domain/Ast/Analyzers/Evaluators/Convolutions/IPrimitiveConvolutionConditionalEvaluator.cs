using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions;

/// <summary>
/// Calculator for convolution operations with conditional operations for struct (primitive) types
/// </summary>
/// <typeparam name="T">Configured conditional expression type (int/real etc.)</typeparam>
public interface IPrimitiveConvolutionConditionalEvaluator
{
    /// <summary>
    /// Calculate conditional expression
    /// </summary>
    /// <param name="expr">Expression node</param>
    /// <returns>Evaluated value. If constant value is not found, returns null.</returns>
    bool? Evaluate( AstExpressionNode expr );
}
