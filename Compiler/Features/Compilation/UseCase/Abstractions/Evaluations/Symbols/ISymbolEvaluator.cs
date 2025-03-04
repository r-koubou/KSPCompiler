using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Symbols;

public interface ISymbolEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstSymbolExpressionNode expr );
}
