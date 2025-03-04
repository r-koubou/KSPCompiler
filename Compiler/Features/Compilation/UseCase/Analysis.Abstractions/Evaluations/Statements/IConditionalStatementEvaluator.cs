using KSPCompiler.Shared.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;

public interface IConditionalStatementEvaluator<in TStatementNode> where TStatementNode : AstStatementNode
{
    IAstNode Evaluate( IAstVisitor visitor, TStatementNode statement );
}
