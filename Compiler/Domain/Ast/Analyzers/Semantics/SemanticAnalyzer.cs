using KSPCompiler.Domain.Ast.Analyzers.Convolutions;
using KSPCompiler.Domain.Ast.Analyzers.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Analyzers.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer : DefaultAstVisitor, ISemanticAnalyzer
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    private ISymbolTable<VariableSymbol> VariableSymbolTable { get; }

    #region Eveluators

    #region Convolution Evaluators

    private IIntegerConvolutionEvaluator IntegerConvolutionEvaluator { get; }
    private RealConvolutionEvaluator RealConvolutionEvaluator { get; }
    private StringConvolutionEvaluator StringConvolutionEvaluator { get; }

    #endregion

    #region Operator Evaluators

    private NumericBinaryOperatorEvaluator NumericBinaryOperatorEvaluator { get; }
    private NumericUnaryOperatorEvaluator NumericUnaryOperatorEvaluator { get; }
    private StringConcatenateOperatorEvaluator StringConcatenateOperatorEvaluator { get; }

    #endregion

    #endregion ~Eveluators

    public SemanticAnalyzer(
        ICompilerMessageManger compilerMessageManger,
        ISymbolTable<VariableSymbol> variableSymbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        VariableSymbolTable   = variableSymbolTable;

        IntegerConvolutionEvaluator        = new IntegerConvolutionEvaluator( this, VariableSymbolTable, CompilerMessageManger );
        RealConvolutionEvaluator           = new RealConvolutionEvaluator( this, VariableSymbolTable, CompilerMessageManger );
        StringConvolutionEvaluator         = new StringConvolutionEvaluator( this, VariableSymbolTable, CompilerMessageManger );

        NumericBinaryOperatorEvaluator     = new NumericBinaryOperatorEvaluator( this, CompilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
        NumericUnaryOperatorEvaluator      = new NumericUnaryOperatorEvaluator( this, CompilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
        StringConcatenateOperatorEvaluator = new StringConcatenateOperatorEvaluator( this, CompilerMessageManger, StringConvolutionEvaluator );
    }

    public void Analyze( AstCompilationUnitNode node, AbortTraverseToken abortTraverseToken)
    {
        node.AcceptChildren( this, abortTraverseToken );
    }
}
