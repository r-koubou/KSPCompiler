using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions;

/// <summary>
/// Interface for evaluating convolution expressions for non-primitive types
/// </summary>
public interface IObjectConvolutionEvaluator<T> where T : class
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
