using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public sealed class IntegerConvolutionEvaluator : IPrimitiveConvolutionEvaluator<int>
{
    private IPrimitiveConstantConvolutionCalculator<int> ConstantCalculator { get; }
    private IPrimitiveConvolutionBinaryCalculator<int> BinaryCalculator { get; }
    private IPrimitiveConvolutionUnaryCalculator<int> UnaryCalculator { get; }
    private IPrimitiveConvolutionConditionalEvaluator<int> ConditionalEvaluator { get; }

    public IntegerConvolutionEvaluator( IAstVisitor visitor, ISymbolTable<VariableSymbol> variableSymbols, ICompilerMessageManger compilerMessageManger )
    {
        ConstantCalculator   = new IntegerConstantConvolutionCalculator( variableSymbols, compilerMessageManger );
        BinaryCalculator     = new IntegerBinaryOperatorConvolutionCalculator( this );
        UnaryCalculator      = new IntegerUnaryOperatorConvolutionCalculator( this );
        ConditionalEvaluator = new IntegerConditionalOperatorConvolutionEvaluator( visitor, this );
    }

    public int? Evaluate( AstExpressionSyntaxNode expr, int workingValueForRecursive )
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
