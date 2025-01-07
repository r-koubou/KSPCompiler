using KSPCompiler.UseCases.Analysis.Evaluations.Preprocessing;
using KSPCompiler.UseCases.Analysis.Evaluations.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;

namespace KSPCompiler.UseCases.Analysis.Context;

public interface IStatementEvaluationContext
{
    IPreprocessEvaluator Preprocess { get; }
    ICallUserFunctionEvaluator CallUserFunction { get; }
    IIfStatementEvaluator If { get; }
    ISelectStatementEvaluator Select { get; }
    IWhileStatementEvaluator While { get; }
    IContinueStatementEvaluator Continue { get; }
}
