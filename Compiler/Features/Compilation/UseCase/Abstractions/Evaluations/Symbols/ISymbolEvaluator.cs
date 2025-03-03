using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Symbols;

public interface ISymbolEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstSymbolExpressionNode expr );
}
