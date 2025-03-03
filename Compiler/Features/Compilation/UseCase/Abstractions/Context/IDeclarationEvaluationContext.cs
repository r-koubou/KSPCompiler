using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.UseCases.Analysis.Context;

public interface IDeclarationEvaluationContext
{
    ICallbackDeclarationEvaluator Callback { get; }
    IUserFunctionDeclarationEvaluator UserFunction { get; }
    IVariableDeclarationEvaluator Variable { get; }
}
