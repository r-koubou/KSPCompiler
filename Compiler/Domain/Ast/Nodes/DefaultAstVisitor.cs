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

    public IAstNode Visit( NullAstExpressionSyntaxNode node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public IAstNode Visit( NullAstInitializer node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstCompilationUnit node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstCallbackDeclaration node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstUserFunctionDeclaration node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstArgument node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstArgumentList node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstBlock node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstCaseBlock node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstLogicalOrExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstLogicalAndExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public IAstNode Visit( AstLogicalXorExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstStringConcatenateExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstBitwiseOrExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstBitwiseAndExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstBitwiseXorExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstEqualExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstNotEqualExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstLessThanExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstGreaterThanExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstLessEqualExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstGreaterEqualExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstAdditionExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstSubtractionExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstMultiplyingExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstDivisionExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstModuloExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstUnaryMinusExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstUnaryNotExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstUnaryLogicalNotExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstIntLiteral node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstRealLiteral node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstStringLiteral node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstExpressionList node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstAssignmentExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstAssignmentExpressionList node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstSymbolExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstArrayElementExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstCallExpression node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstKspPreprocessorDefine node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstKspPreprocessorUndefine node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstKspPreprocessorIfdefine node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstKspPreprocessorIfnotDefine node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstIfStatement node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstWhileStatement node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstSelectStatement node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstCallKspUserFunctionStatement node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstVariableDeclaration node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstVariableInitializer node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstPrimitiveInitializer node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstArrayInitializer node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );

    public virtual IAstNode Visit( AstContinueStatement node, AbortTraverseToken abortTraverseToken )
        => VisitChildren( node, abortTraverseToken );
}
