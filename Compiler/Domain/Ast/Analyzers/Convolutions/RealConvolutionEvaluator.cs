using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public sealed class RealConvolutionEvaluator : IPrimitiveConvolutionEvaluator<double>
{
    private IPrimitiveConstantConvolutionCalculator<double> ConstantCalculator { get; }
    private IPrimitiveConvolutionBinaryCalculator<double> BinaryCalculator { get; }
    private IPrimitiveConvolutionUnaryCalculator<double> UnaryCalculator { get; }
    private IPrimitiveConvolutionConditionalEvaluator<double> ConditionalEvaluator { get; }

    public RealConvolutionEvaluator( IAstVisitor visitor, ISymbolTable<VariableSymbol> variableSymbols, ICompilerMessageManger compilerMessageManger )
    {
        ConstantCalculator   = new RealConstantConvolutionCalculator( variableSymbols, compilerMessageManger );
        BinaryCalculator     = new RealBinaryOperatorConvolutionCalculator( this );
        UnaryCalculator      = new RealUnaryOperatorConvolutionCalculator( this );
        ConditionalEvaluator = new RealConditionalOperatorConvolutionEvaluator( visitor, this );
    }

    public double? Evaluate( AstExpressionSyntaxNode expr, double workingValueForRecursive )
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
