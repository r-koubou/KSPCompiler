using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

public interface ISymbolEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstSymbolExpressionNode expr );
}
