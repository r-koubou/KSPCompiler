using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;

namespace KSPCompiler.Domain.Ast.Analyzers.Context;

public interface IDeclarationEvaluationContext
{
    ICallbackDeclarationEvaluator Callback { get; }
    IUserFunctionDeclarationEvaluator UserFunction { get; }
    IVariableDeclarationEvaluator Variable { get; }
}
