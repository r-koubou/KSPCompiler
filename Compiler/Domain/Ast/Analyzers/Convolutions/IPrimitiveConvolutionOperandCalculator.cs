using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with operands for struct (primitive) types
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPrimitiveConvolutionOperandCalculator<T> : IPrimitiveConvolutionCalculator<T> where T : struct {}

public sealed class NullConvolutionOperandCalculator<T> : IPrimitiveConvolutionCalculator<T> where T : struct
{
    public T? Calculate( AstExpressionSyntaxNode expr, T workingValueForRecursive )
        => null;
}
