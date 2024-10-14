using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer : DefaultAstVisitor, ISemanticAnalyzer
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    private AggregateSymbolTable SymbolTable { get; }

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
    private IAssignOperatorEvaluator AssignOperatorEvaluator { get; }

    #endregion

    #region Symbol Evaluators

    private ISymbolEvaluator SymbolEvaluator { get; }

    #endregion

    #endregion ~Eveluators

    public SemanticAnalyzer(
        ICompilerMessageManger compilerMessageManger,
        AggregateSymbolTable symbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        SymbolTable           = symbolTable;

        IntegerConvolutionEvaluator = new IntegerConvolutionEvaluator( this, SymbolTable.Variables, CompilerMessageManger );
        RealConvolutionEvaluator    = new RealConvolutionEvaluator( this, SymbolTable.Variables, CompilerMessageManger );
        StringConvolutionEvaluator  = new StringConvolutionEvaluator( this, SymbolTable.Variables, CompilerMessageManger );

        NumericBinaryOperatorEvaluator     = new NumericBinaryOperatorEvaluator( this, CompilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
        NumericUnaryOperatorEvaluator      = new NumericUnaryOperatorEvaluator( this, CompilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
        StringConcatenateOperatorEvaluator = new StringConcatenateOperatorEvaluator( this, CompilerMessageManger, StringConvolutionEvaluator );
        AssignOperatorEvaluator            = new AssignOperatorEvaluator( this, CompilerMessageManger, SymbolTable.Variables );

        SymbolEvaluator                    = new SymbolEvaluator( this, CompilerMessageManger, SymbolTable );
    }

    public void Analyze( AstCompilationUnitNode node, AbortTraverseToken abortTraverseToken)
    {
        node.AcceptChildren( this, abortTraverseToken );
    }
}
