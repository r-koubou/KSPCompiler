using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions;

/// <summary>
/// Base interface for convolution calculators for struct (primitive) types
/// </summary>
/// <typeparam name="T">Calculator return type</typeparam>
public interface IPrimitiveConvolutionCalculator<T> where T : struct
{
    /// <summary>
    /// Calculate the value of the expression
    /// </summary>
    /// <param name="visitor">Visitor for traversing the AST</param>
    /// <param name="expr">Expression node</param>
    /// <param name="workingValueForRecursive">Working value for recursive evaluation. First call should be default value of `T`.</param>
    /// <returns>Calculated value. If constant value is not found, returns null.</returns>
    T? Calculate( IAstVisitor visitor, AstExpressionNode expr, T workingValueForRecursive );
}
