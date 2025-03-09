using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

public abstract class DefaultAstVisitor : IAstVisitor
{
    public virtual IAstNode VisitChildren( IAstNode node )
    {
        node.AcceptChildren( this );

        return node;
    }

    #region Null AstNode
    public virtual IAstNode Visit( NullAstNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( NullAstModiferNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( NullAstBlockNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( NullAstCaseBlockNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( NullAstExpressionNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( NullAstExpressionListNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( NullAstVariableInitializerNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( NullAstPrimitiveInitializerNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( NullAstArrayInitializerNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCompilationUnitNode node )
        => VisitChildren( node );
    #endregion ~Null AstNode

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

    public virtual IAstNode Visit( AstAssignmentExpressionNode node )
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

    public virtual IAstNode Visit( AstPreprocessorDefineNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstPreprocessorUndefineNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstPreprocessorIfdefineNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstPreprocessorIfnotDefineNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstIfStatementNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstWhileStatementNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstSelectStatementNode node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCallUserFunctionStatementNode node )
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
