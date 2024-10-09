using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with unary operations
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IConvolutionUnaryCalculator<T> : IConvolutionCalculator<T> where T : struct {}

public sealed class  NullConvolutionUnaryCalculator<T> : IConvolutionCalculator<T> where T : struct
{
    public T? Calculate( AstExpressionSyntaxNode expr, T workingValueForRecursive )
        => null;
}
