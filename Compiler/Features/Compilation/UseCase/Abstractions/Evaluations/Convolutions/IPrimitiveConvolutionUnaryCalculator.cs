using KSPCompiler.Shared.Domain.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Convolutions;

/// <summary>
/// Calculator for convolution operations with unary operations for struct (primitive) types
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPrimitiveConvolutionUnaryCalculator<T> : IPrimitiveConvolutionCalculator<T> where T : struct {}

public sealed class  NullConvolutionUnaryCalculator<T> : IPrimitiveConvolutionCalculator<T> where T : struct
{
    public T? Calculate( IAstVisitor visitor, AstExpressionNode expr, T workingValueForRecursive )
        => null;
}
