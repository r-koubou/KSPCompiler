using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public sealed class IntegerConvolutionEvaluator : IIntegerConvolutionEvaluator
{
    private IIntegerConstantConvolutionCalculator ConstantCalculator { get; }
    private IIntegerBinaryOperatorConvolutionCalculator BinaryCalculator { get; }
    private IIntegerUnaryOperatorConvolutionCalculator UnaryCalculator { get; }

    public IntegerConvolutionEvaluator( IAstVisitor visitor, IVariableSymbolTable variableSymbols, ICompilerMessageManger compilerMessageManger )
    {
        ConstantCalculator   = new IntegerConstantConvolutionCalculator( variableSymbols, compilerMessageManger );
        BinaryCalculator     = new IntegerBinaryOperatorConvolutionCalculator( this );
        UnaryCalculator      = new IntegerUnaryOperatorConvolutionCalculator( this );
    }

    public int? Evaluate( IAstVisitor visitor, AstExpressionNode expr, int workingValueForRecursive )
    {
        return expr.ChildNodeCount switch
        {
            0 => ConstantCalculator.Calculate( visitor, expr, workingValueForRecursive ),
            1 => UnaryCalculator.Calculate( visitor, expr, workingValueForRecursive ),
            2 => BinaryCalculator.Calculate( visitor, expr, workingValueForRecursive ),
            _ => null
        };
    }
}
