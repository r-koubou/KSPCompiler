using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Convolutions;

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
