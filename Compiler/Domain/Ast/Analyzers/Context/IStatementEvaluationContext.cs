using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Preprocessing;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.UserFunctions;

namespace KSPCompiler.Domain.Ast.Analyzers.Context;

public interface IStatementEvaluationContext
{
    IPreprocessEvaluator Preprocess { get; }
    ICallUserFunctionEvaluator CallUserFunction { get; }
    IContinueStatementEvaluator Continue { get; }
    IIfStatementEvaluator If { get; }
    ISelectStatementEvaluator Select { get; }
    IWhileStatementEvaluator While { get; }
}
