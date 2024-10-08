using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public sealed class IntegerConvolutionEvaluator : IConvolutionEvaluator<int>
{
    private IConvolutionOperandCalculator<int> OperandCalculator { get; }
    private IConvolutionBinaryCalculator<int> BinaryCalculator { get; }
    private IConvolutionUnaryCalculator<int> UnaryCalculator { get; }
    private IConvolutionConditionalEvaluator<int> ConditionalEvaluator { get; }

    public IntegerConvolutionEvaluator( IAstVisitor visitor, ISymbolTable<VariableSymbol> variableSymbols, ICompilerMessageManger compilerMessageManger )
    {
        OperandCalculator    = new IntegerConvolutionOperandCalculator( variableSymbols, compilerMessageManger );
        BinaryCalculator     = new IntegerConvolutionBinaryCalculator( this );
        UnaryCalculator      = new IntegerConvolutionUnaryCalculator( this );
        ConditionalEvaluator = new IntegerConvolutionConditionalEvaluator( visitor, this );
    }

    public int? Evaluate( AstExpressionSyntaxNode expr, int workingValueForRecursive )
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
