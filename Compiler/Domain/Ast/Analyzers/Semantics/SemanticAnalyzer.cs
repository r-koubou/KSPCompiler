using KSPCompiler.Domain.Ast.Analyzers.Context;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer : DefaultAstVisitor, IAstTraversal
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private AggregateSymbolTable SymbolTable { get; }
    private IAnalyzerContext Context { get; }


    public SemanticAnalyzer(
        IAnalyzerContext context,
        AggregateSymbolTable symbolTable,
        ICompilerMessageManger compilerMessageManger )
    {
        Context               = context;
        SymbolTable           = symbolTable;
        CompilerMessageManger = compilerMessageManger;

/*
        CallbackDeclarationEvaluator     = new CallbackDeclarationEvaluator( CompilerMessageManger, SymbolTable.ReservedCallbacks, SymbolTable.UserCallbacks );
        UserFunctionDeclarationEvaluator = new UserFunctionDeclarationEvaluator( CompilerMessageManger, SymbolTable.UserFunctions );
        VariableDeclarationEvaluator     = new VariableDeclarationEvaluator( CompilerMessageManger, SymbolTable.Variables, SymbolTable.UITypes );

        IntegerConvolutionEvaluator = new IntegerConvolutionEvaluator();
        RealConvolutionEvaluator    = new RealConvolutionEvaluator();
        //StringConvolutionEvaluator  = new StringConvolutionEvaluator( this, SymbolTable.Variables, CompilerMessageManger );

        NumericBinaryOperatorEvaluator     = new NumericBinaryOperatorEvaluator( CompilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
        NumericUnaryOperatorEvaluator      = new NumericUnaryOperatorEvaluator( CompilerMessageManger, IntegerConvolutionEvaluator, RealConvolutionEvaluator );
        StringConcatenateOperatorEvaluator = new StringConcatenateOperatorEvaluator( CompilerMessageManger, StringConvolutionEvaluator );

        SymbolEvaluator                    = new SymbolEvaluator( CompilerMessageManger, SymbolTable );
        AssignOperatorEvaluator            = new AssignOperatorEvaluator( CompilerMessageManger );
*/
    }


    public void Traverse( AstCompilationUnitNode node )
    {
        node.AcceptChildren( this );
    }
}
