using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Context;

public interface IDeclarationEvaluationContext
{
    ICallbackDeclarationEvaluator Callback { get; }
    IUserFunctionDeclarationEvaluator UserFunction { get; }
    IVariableDeclarationEvaluator Variable { get; }
}
