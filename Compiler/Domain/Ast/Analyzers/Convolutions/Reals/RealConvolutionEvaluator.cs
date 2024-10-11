using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions.Reals;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public sealed class RealConvolutionEvaluator : IRealConvolutionEvaluator
{
    private IRealConstantConvolutionCalculator ConstantCalculator { get; }
    private IRealBinaryOperatorConvolutionCalculator BinaryCalculator { get; }
    private IRealUnaryOperatorConvolutionCalculator UnaryCalculator { get; }
    private IRealConditionalOperatorConvolutionEvaluator ConditionalEvaluator { get; }

    public RealConvolutionEvaluator( IAstVisitor visitor, ISymbolTable<VariableSymbol> variableSymbols, ICompilerMessageManger compilerMessageManger )
    {
        ConstantCalculator   = new RealConstantConvolutionCalculator( variableSymbols, compilerMessageManger );
        BinaryCalculator     = new RealBinaryOperatorConvolutionCalculator( this );
        UnaryCalculator      = new RealUnaryOperatorConvolutionCalculator( this );
        ConditionalEvaluator = new RealConditionalOperatorConvolutionEvaluator( visitor, this );
    }

    public double? Evaluate( AstExpressionNode expr, double workingValueForRecursive )
    {
        return expr.ChildNodeCount switch
        {
            0 => ConstantCalculator.Calculate( expr, workingValueForRecursive ),
            1 => UnaryCalculator.Calculate( expr, workingValueForRecursive ),
            2 => BinaryCalculator.Calculate( expr, workingValueForRecursive ),
            _ => null
        };
    }
}
