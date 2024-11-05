using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Commands;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Context;

public interface IExpressionEvaluatorContext
{
    IAssignOperatorEvaluator AssignOperator { get; }
    IConditionalBinaryOperatorEvaluator ConditionalBinaryOperator { get; }
    IConditionalLogicalOperatorEvaluator ConditionalLogicalOperator { get; }
    IUnaryOperatorEvaluator ConditionalUnaryOperator { get; }
    IBinaryOperatorEvaluator NumericBinaryOperator { get; }
    IUnaryOperatorEvaluator NumericUnaryOperator { get; }
    IStringConcatenateOperatorEvaluator StringConcatenateOperator { get; }
    ISymbolEvaluator Symbol { get; }
    IArrayElementEvaluator ArrayElement { get; }
    ICallCommandExpressionEvaluator CallCommand { get; }
}
