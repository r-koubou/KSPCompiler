using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;

/// <summary>
/// Evaluating string convolution expressions
/// </summary>
public sealed class StringConvolutionEvaluator : IStringConvolutionEvaluator
{
    private IStringConstantConvolutionCalculator ConstantCalculator { get; }
    private IStringConcatenateOperatorConvolutionCalculator ConcatenateCalculator { get; }

    public StringConvolutionEvaluator( IAstVisitor visitor, IVariableSymbolTable variableSymbols, ICompilerMessageManger compilerMessageManger )
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
