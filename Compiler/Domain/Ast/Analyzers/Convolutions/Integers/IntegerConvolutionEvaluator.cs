using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions.Integers;

/// <summary>
/// Interface for evaluating convolution expressions
/// </summary>
public sealed class IntegerConvolutionEvaluator : IIntegerConvolutionEvaluator
{
    private IIntegerConstantConvolutionCalculator ConstantCalculator { get; }
    private IIntegerBinaryOperatorConvolutionCalculator BinaryCalculator { get; }
    private IIntegerUnaryOperatorConvolutionCalculator UnaryCalculator { get; }
    private IntegerConditionalOperatorConvolutionEvaluator ConditionalEvaluator { get; }

    public IntegerConvolutionEvaluator( IAstVisitor visitor, ISymbolTable<VariableSymbol> variableSymbols, ICompilerMessageManger compilerMessageManger )
    {
        ConstantCalculator   = new IntegerConstantConvolutionCalculator( variableSymbols, compilerMessageManger );
        BinaryCalculator     = new IntegerBinaryOperatorConvolutionCalculator( this );
        UnaryCalculator      = new IntegerUnaryOperatorConvolutionCalculator( this );
        ConditionalEvaluator = new IntegerConditionalOperatorConvolutionEvaluator( visitor, this );
    }

    public int? Evaluate( AstExpressionNode expr, int workingValueForRecursive )
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
