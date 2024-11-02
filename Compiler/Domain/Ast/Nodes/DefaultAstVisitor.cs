using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Nodes;

public abstract class DefaultAstVisitor : IAstVisitor
{
    public virtual IAstNode VisitChildren( IAstNode node )
    {
        node.AcceptChildren( this );

        return node;
    }

    public virtual IAstNode Visit( NullAstNode node )
        => VisitChildren( node );

    public IAstNode Visit( NullAstExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCompilationUnitNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCallbackDeclarationNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstUserFunctionDeclarationNode node )
        => VisitChildren( node );

    public IAstNode Visit( AstModiferNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstArgumentNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstArgumentListNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstBlockNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCaseBlock node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstLogicalOrExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstLogicalAndExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstLogicalXorExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstStringConcatenateExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstBitwiseOrExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstBitwiseAndExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstBitwiseXorExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstEqualExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstNotEqualExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstLessThanExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstGreaterThanExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstLessEqualExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstGreaterEqualExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstAdditionExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstSubtractionExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstMultiplyingExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstDivisionExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstModuloExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstUnaryMinusExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstUnaryNotExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstUnaryLogicalNotExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstIntLiteralNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstRealLiteralNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstStringLiteralNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstBooleanLiteralNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstExpressionListNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstAssignStatementNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstAssignmentExpressionListNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstDefaultExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstSymbolExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstArrayElementExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCallCommandExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstKspPreprocessorDefineNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstKspPreprocessorUndefineNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstKspPreprocessorIfdefineNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstKspPreprocessorIfnotDefineNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstIfStatementNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstWhileStatementNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstSelectStatementNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCallKspUserFunctionStatementNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstVariableDeclarationNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstVariableInitializerNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstPrimitiveInitializerNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstArrayInitializerNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstContinueStatementNode node )
        => VisitChildren( node );
}
