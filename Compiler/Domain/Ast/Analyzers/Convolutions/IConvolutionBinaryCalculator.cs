using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with binary operations
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IConvolutionBinaryCalculator<T> : IConvolutionCalculator<T> where T : struct {}

public sealed class NullConvolutionBinaryCalculator<T> : IConvolutionCalculator<T> where T : struct
{
    public T? Calculate( AstExpressionSyntaxNode expr, T workingValueForRecursive )
        => null;
}
