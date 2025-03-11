using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;

public interface IContinueStatementEvaluator : IConditionalStatementEvaluator<AstContinueStatementNode> {}
