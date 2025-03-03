using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Declarations;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Context;

public interface IDeclarationEvaluationContext
{
    ICallbackDeclarationEvaluator Callback { get; }
    IUserFunctionDeclarationEvaluator UserFunction { get; }
    IVariableDeclarationEvaluator Variable { get; }
}
