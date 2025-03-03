using KSPCompiler.UseCases.Analysis.Evaluations.Commands;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Booleans;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Integers;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Reals;
using KSPCompiler.UseCases.Analysis.Evaluations.Convolutions.Strings;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

namespace KSPCompiler.UseCases.Analysis.Context;

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
    IBinaryOperatorEvaluator NumericBinaryOperator { get; }
    IUnaryOperatorEvaluator NumericUnaryOperator { get; }
    IStringConcatenateOperatorEvaluator StringConcatenateOperator { get; }
    IConditionalBinaryOperatorEvaluator ConditionalBinaryOperator { get; }
    IConditionalLogicalOperatorEvaluator ConditionalLogicalOperator { get; }
    IUnaryOperatorEvaluator ConditionalUnaryOperator { get; }
    ISymbolEvaluator Symbol { get; }
    IArrayElementEvaluator ArrayElement { get; }
    ICallCommandEvaluator CallCommand { get; }

    #endregion ~Expression Evaluators
}
