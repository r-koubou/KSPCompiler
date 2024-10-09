using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with unary operations for struct (primitive) types
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPrimitiveConvolutionUnaryCalculator<T> : IPrimitiveConvolutionCalculator<T> where T : struct {}

public sealed class  NullConvolutionUnaryCalculator<T> : IPrimitiveConvolutionCalculator<T> where T : struct
{
    public T? Calculate( AstExpressionSyntaxNode expr, T workingValueForRecursive )
        => null;
}
