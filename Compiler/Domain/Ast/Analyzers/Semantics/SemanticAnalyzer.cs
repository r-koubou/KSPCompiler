using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer : DefaultAstVisitor, IAstTraversal
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    private AggregateSymbolTable SymbolTable { get; }

    #region Declaration

    private ICallbackDeclarationEvaluator CallbackDeclarationEvaluator { get; }
    private IUserFunctionDeclarationEvaluator UserFunctionDeclarationEvaluator { get; }
    private IVariableDeclarationEvaluator VariableDeclarationEvaluator { get; }

    #endregion

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
    private IArrayElementEvaluator ArrayElementEvaluator { get; }

    #endregion

    #endregion ~Eveluators

    public SemanticAnalyzer(
        ICompilerMessageManger compilerMessageManger,
        AggregateSymbolTable symbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        SymbolTable           = symbolTable;

        CallbackDeclarationEvaluator     = new CallbackDeclarationEvaluator( CompilerMessageManger, SymbolTable.ReservedCallbacks, SymbolTable.UserCallbacks );
        UserFunctionDeclarationEvaluator = new UserFunctionDeclarationEvaluator( CompilerMessageManger, SymbolTable.UserFunctions );
        VariableDeclarationEvaluator     = new VariableDeclarationEvaluator( CompilerMessageManger, SymbolTable.Variables, SymbolTable.UITypes );

        IntegerConvolutionEvaluator = new IntegerConvolutionEvaluator( this, SymbolTable.Variables, CompilerMessageManger );
        RealConvolutionEvaluator    = new RealConvolutionEvaluator( this, SymbolTable.Variables, CompilerMessageManger );
        StringConvolutionEvaluator  = new StringConvolutionEvaluator( this, SymbolTable.Variables, CompilerMessageManger );

        NumericBinaryOperatorEvaluator     = new NumericBinaryOperatorEvaluator( this, CompilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
        NumericUnaryOperatorEvaluator      = new NumericUnaryOperatorEvaluator( this, CompilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
        StringConcatenateOperatorEvaluator = new StringConcatenateOperatorEvaluator( this, CompilerMessageManger, StringConvolutionEvaluator );

        SymbolEvaluator                    = new SymbolEvaluator( CompilerMessageManger, SymbolTable );
        AssignOperatorEvaluator            = new AssignOperatorEvaluator( CompilerMessageManger );
    }

    public void Traverse( AstCompilationUnitNode node )
    {
        node.AcceptChildren( this );
    }
}
