using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Evaluating string convolution expressions
/// </summary>
public sealed class StringConvolutionEvaluator : IStringConvolutionEvaluator
{
    private IObjectConvolutionCalculator<string> ConstantCalculator { get; }
    private IObjectConvolutionCalculator<string> ConcatenateCalculator { get; }

    public StringConvolutionEvaluator( IAstVisitor visitor, ISymbolTable<VariableSymbol> variableSymbols, ICompilerMessageManger compilerMessageManger )
    {
        ConstantCalculator    = new StringConstantConvolutionCalculator( variableSymbols, compilerMessageManger );
        ConcatenateCalculator = new StringConcatenateOperatorConvolutionCalculator( this );
    }

    public string? Evaluate( AstExpressionNode expr, string workingValueForRecursive )
    {
        return expr.ChildNodeCount switch
        {
            0 => ConstantCalculator.Calculate( expr, workingValueForRecursive ),
            2 => ConcatenateCalculator.Calculate( expr, workingValueForRecursive ),
            _ => null
        };
    }
}
