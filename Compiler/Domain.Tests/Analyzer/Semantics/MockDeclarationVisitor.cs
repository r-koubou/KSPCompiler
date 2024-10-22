using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockDeclarationVisitor : DefaultAstVisitor
{
    private IBinaryOperatorEvaluator NumericBinaryOperatorEvaluator { get; set; } = new MockBinaryOperatorEvaluator();
    private IVariableDeclarationEvaluator VariableDeclarationEvaluator { get; set; } = new MockVariableDeclarationEvaluator();
    private ICallbackDeclarationEvaluator CallbackDeclarationEvaluator { get; set; } = new MockCallbackDeclarationEvaluator();
    private IUserFunctionDeclarationEvaluator UserFunctionDeclarationEvaluator { get; set; } = new MockUserFunctionDeclarationEvaluator();

    public void Inject( IBinaryOperatorEvaluator binaryOperatorEvaluator )
    {
        NumericBinaryOperatorEvaluator = binaryOperatorEvaluator;
    }

    public void Inject( IVariableDeclarationEvaluator variableDeclarationEvaluator )
    {
        VariableDeclarationEvaluator = variableDeclarationEvaluator;
    }

    public void Inject( ICallbackDeclarationEvaluator callbackDeclarationEvaluator )
    {
        CallbackDeclarationEvaluator = callbackDeclarationEvaluator;
    }

    public void Inject( IUserFunctionDeclarationEvaluator userFunctionDeclarationEvaluator )
    {
        UserFunctionDeclarationEvaluator = userFunctionDeclarationEvaluator;
    }

    #region Binary Operators (Mathematical)
    public override IAstNode Visit( AstAdditionExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstSubtractionExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstMultiplyingExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstDivisionExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstModuloExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );
    #endregion ~Binary Operators

    #region Binary Operators (Bitwise)
    public override IAstNode Visit( AstBitwiseOrExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstBitwiseAndExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstBitwiseXorExpressionNode node )
        => NumericBinaryOperatorEvaluator.Evaluate( this, node );
    #endregion ~Binary Operators (Bitwise)
}
