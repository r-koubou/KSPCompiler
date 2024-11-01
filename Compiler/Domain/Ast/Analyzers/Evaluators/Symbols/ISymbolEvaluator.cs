using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;

public interface ISymbolEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstSymbolExpressionNode expr );
}
