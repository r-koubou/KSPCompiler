using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Convolutions;

/// <summary>
/// Calculator for convolution operations with binary operations for struct (primitive) types
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPrimitiveConvolutionBinaryCalculator<T> : IPrimitiveConvolutionCalculator<T> where T : struct {}

public sealed class NullConvolutionBinaryCalculator<T> : IPrimitiveConvolutionCalculator<T> where T : struct
{
    public T? Calculate( IAstVisitor visitor, AstExpressionNode expr, T workingValueForRecursive )
        => null;
}
