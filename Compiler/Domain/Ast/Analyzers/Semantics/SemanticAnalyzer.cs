using KSPCompiler.Domain.Ast.Analyzers.Convolutions;
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

    private NumericBinaryOperatorEvaluator NumericBinaryOperatorEvaluator { get; }
    private NumericUnaryOperatorEvaluator NumericUnaryOperatorEvaluator { get; }
    private IntegerConvolutionEvaluator IntegerConvolutionEvaluator { get; }
    private RealConvolutionEvaluator RealConvolutionEvaluator { get; }

    #endregion ~Eveluators

    public SemanticAnalyzer(
        ICompilerMessageManger compilerMessageManger,
        ISymbolTable<VariableSymbol> variableSymbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        VariableSymbolTable   = variableSymbolTable;

        IntegerConvolutionEvaluator    = new IntegerConvolutionEvaluator( this, VariableSymbolTable, CompilerMessageManger );
        RealConvolutionEvaluator       = new RealConvolutionEvaluator( this, VariableSymbolTable, CompilerMessageManger );
        NumericBinaryOperatorEvaluator = new NumericBinaryOperatorEvaluator( this, CompilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
        NumericUnaryOperatorEvaluator  = new NumericUnaryOperatorEvaluator( this, CompilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
    }

    public void Analyze( AstCompilationUnitNode node, AbortTraverseToken abortTraverseToken)
    {
        node.AcceptChildren( this, abortTraverseToken );
    }
}
