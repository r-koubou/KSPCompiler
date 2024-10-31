namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Booleans;

/// <summary>
/// Calculator for convolution operations with boolean expressions
/// </summary>
public interface IConditionalLogicalOperatorConvolutionCalculator : IConditionalConvolutionCalculator {}

public interface IBooleanConvolutionEvaluator : IPrimitiveConvolutionEvaluator<bool> {}
