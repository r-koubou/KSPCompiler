using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;

public interface ISymbolEvaluator<TEvalResult>
{
    public TEvalResult Evaluate( IAstVisitor<TEvalResult> visitor, AstSymbolExpressionNode expr );
}

public interface ISymbolEvaluator : ISymbolEvaluator<IAstNode> {}
