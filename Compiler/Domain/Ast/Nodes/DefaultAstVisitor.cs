using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Nodes;

public abstract class DefaultAstVisitor : IAstVisitor
{
    public virtual IAstNode VisitChildren( IAstNode node, AbortTraverseToken abortTraverseToken )
    {
        if( abortTraverseToken.Aborted )
        {
            return node;
        }

        node.AcceptChildren( this, abortTraverseToken );

        return node;
    }

    public virtual IAstNode Visit( NullAstNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public IAstNode Visit( NullAstExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public IAstNode Visit( NullAstInitializerNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstCompilationUnitNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstCallbackDeclarationNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstUserFunctionDeclarationNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstArgumentNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstArgumentListNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstBlockNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstCaseBlock node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstLogicalOrExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstLogicalAndExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public IAstNode Visit( AstLogicalXorExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstStringConcatenateExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstBitwiseOrExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstBitwiseAndExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstBitwiseXorExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstEqualExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstNotEqualExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstLessThanExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstGreaterThanExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstLessEqualExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstGreaterEqualExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstAdditionExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstSubtractionExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstMultiplyingExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstDivisionExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstModuloExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstUnaryMinusExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstUnaryNotExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstUnaryLogicalNotExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstIntLiteralNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstRealLiteralNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstStringLiteralNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstExpressionListNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstAssignmentExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstAssignmentExpressionListNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstDefaultExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstSymbolExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstArrayElementExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstCallCommandExpressionNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstKspPreprocessorDefineNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstKspPreprocessorUndefineNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstKspPreprocessorIfdefineNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstKspPreprocessorIfnotDefineNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstIfStatementNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstWhileStatementNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstSelectStatementNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstCallKspUserFunctionStatementNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstVariableDeclarationNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstVariableInitializerNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstPrimitiveInitializerNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstArrayInitializerNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstContinueStatementNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );
}
