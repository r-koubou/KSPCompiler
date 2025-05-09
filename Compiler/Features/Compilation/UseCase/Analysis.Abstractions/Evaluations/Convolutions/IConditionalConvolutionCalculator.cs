using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions;

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
