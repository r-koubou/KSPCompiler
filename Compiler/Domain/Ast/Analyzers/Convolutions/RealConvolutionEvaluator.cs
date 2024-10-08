using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public sealed class RealConvolutionEvaluator : IConvolutionEvaluator<double>
{
    private IConvolutionOperandCalculator<double> OperandCalculator { get; }
    private IConvolutionBinaryCalculator<double> BinaryCalculator { get; }
    private IConvolutionUnaryCalculator<double> UnaryCalculator { get; }
    private IConvolutionConditionalEvaluator<double> ConditionalEvaluator { get; }

    public RealConvolutionEvaluator( IAstVisitor visitor, ISymbolTable<VariableSymbol> variableSymbols, ICompilerMessageManger compilerMessageManger, AbortTraverseToken abortTraverseToken )
    {
        OperandCalculator    = new RealConvolutionOperandCalculator( variableSymbols, compilerMessageManger );
        BinaryCalculator     = new RealConvolutionBinaryCalculator( this );
        UnaryCalculator      = new RealConvolutionUnaryCalculator( this );
        ConditionalEvaluator = new RealConvolutionConditionalEvaluator( visitor, abortTraverseToken, this );
    }

    public double? Evaluate( AstExpressionSyntaxNode expr, double workingValueForRecursive )
    {
        return expr.ChildNodeCount switch
        {
            0 => OperandCalculator.Calculate( expr, workingValueForRecursive ),
            1 => UnaryCalculator.Calculate( expr, workingValueForRecursive ),
            2 => BinaryCalculator.Calculate( expr, workingValueForRecursive ),
            _ => null
        };
    }
}
