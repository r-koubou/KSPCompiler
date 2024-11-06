using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Convolutions;

/// <summary>
/// Calculator for convolution operations with conditional operations
/// </summary>
public interface IConditionalConvolutionCalculator
{
    /// <summary>
    /// Calculate conditional expression
    /// </summary>
    /// <param name="visitor">Visitor for traversing the AST</param>
    /// <param name="expr">Expression node</param>
    /// <returns>Evaluated value. If constant value is not found, returns null.</returns>
    bool? Calculate( IAstVisitor visitor, AstExpressionNode expr );
}
