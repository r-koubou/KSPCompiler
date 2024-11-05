using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Commands;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Context;

public interface IExpressionEvaluatorContext
{
    #region Convolution Evaluators

    IIntegerConvolutionEvaluator IntegerConvolutionEvaluator { get; }
    IRealConvolutionEvaluator RealConvolutionEvaluator { get; }
    IStringConvolutionEvaluator StringConvolutionEvaluator { get; }
    IBooleanConvolutionEvaluator BooleanConvolutionEvaluator { get; }

    #endregion ~Convolution Evaluators

    #region Expression Evaluators

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

    #endregion ~Expression Evaluators
}
