using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Statements;

public interface IContinueStatementEvaluator : IConditionalStatementEvaluator<AstContinueStatementNode> {}
