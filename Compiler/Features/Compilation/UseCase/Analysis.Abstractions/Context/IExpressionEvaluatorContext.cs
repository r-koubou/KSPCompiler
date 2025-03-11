using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Commands;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Booleans;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Integers;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Reals;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions.Strings;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Context;

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
