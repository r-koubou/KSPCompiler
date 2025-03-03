using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Statements;

public interface ISelectStatementEvaluator : IConditionalStatementEvaluator<AstSelectStatementNode> {}
