using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators;

public interface IVariableInitializationEvaluator : IOperatorEvaluator<IAstNode> {}
