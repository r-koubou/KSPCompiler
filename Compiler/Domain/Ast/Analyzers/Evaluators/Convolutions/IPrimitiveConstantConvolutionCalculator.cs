namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions;

/// <summary>
/// Calculator for convolution operations with constant/literal for struct (primitive) types
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPrimitiveConstantConvolutionCalculator<T> : IPrimitiveConvolutionCalculator<T> where T : struct {}
