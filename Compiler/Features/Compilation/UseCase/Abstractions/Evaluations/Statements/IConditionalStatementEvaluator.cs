using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Statements;

public interface IConditionalStatementEvaluator<in TStatementNode> where TStatementNode : AstStatementNode
{
    IAstNode Evaluate( IAstVisitor visitor, TStatementNode statement );
}
