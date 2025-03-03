using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Convolutions;

/// <summary>
/// Interface for evaluating convolution expressions for struct (primitive) types
/// </summary>
public interface IPrimitiveConvolutionEvaluator<T> where T : struct
{
    /// <summary>
    /// Evaluate expression node
    /// </summary>
    /// <param name="visitor">Visitor for traversing the AST</param>
    /// <param name="expr">Expression node</param>
    /// <param name="workingValueForRecursive">Working value for recursive evaluation. First call should be default value of T</param>
    /// <returns>Evaluated value. If constant value is not found, returns null.</returns>
    T? Evaluate( IAstVisitor visitor, AstExpressionNode expr, T workingValueForRecursive );
}
