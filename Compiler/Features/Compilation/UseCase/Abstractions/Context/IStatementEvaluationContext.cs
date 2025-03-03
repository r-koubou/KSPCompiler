using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Preprocessing;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Statements;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.UserFunctions;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Context;

public interface IStatementEvaluationContext
{
    IPreprocessEvaluator Preprocess { get; }
    ICallUserFunctionEvaluator CallUserFunction { get; }
    IIfStatementEvaluator If { get; }
    ISelectStatementEvaluator Select { get; }
    IWhileStatementEvaluator While { get; }
    IContinueStatementEvaluator Continue { get; }
}
