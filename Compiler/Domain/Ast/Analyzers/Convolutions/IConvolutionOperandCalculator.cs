using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with operands
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IConvolutionOperandCalculator<T> : IConvolutionCalculator<T> where T : struct {}

public sealed class NullConvolutionOperandCalculator<T> : IConvolutionCalculator<T> where T : struct
{
    public T? Calculate( AstExpressionSyntaxNode expr, T workingValueForRecursive )
        => null;
}
