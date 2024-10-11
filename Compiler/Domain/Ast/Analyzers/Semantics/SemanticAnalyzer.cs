using KSPCompiler.Domain.Ast.Analyzers.Evaluators;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
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
    private IRealConvolutionEvaluator RealConvolutionEvaluator { get; }
    private IStringConvolutionEvaluator StringConvolutionEvaluator { get; }

    #endregion

    #region Operator Evaluators

    private IBinaryOperatorEvaluator NumericBinaryOperatorEvaluator { get; }
    private IUnaryOperatorEvaluator NumericUnaryOperatorEvaluator { get; }
    private IStringConcatenateOperatorEvaluator StringConcatenateOperatorEvaluator { get; }

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
