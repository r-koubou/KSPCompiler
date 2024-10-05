using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Nodes;

public abstract class DefaultAstVisitor : IAstVisitor<IAstNode>
{
    public virtual IAstNode VisitChildren( IAstNode node )
    {
        node.AcceptChildren( this );
        return node;
    }

    public virtual IAstNode Visit( NullAstNode node )
        => VisitChildren( node );

    public IAstNode Visit( NullAstExpressionSyntaxNode node )
        => VisitChildren( node );

    public IAstNode Visit( NullAstInitializer node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCompilationUnit node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCallbackDeclaration node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstUserFunctionDeclaration node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstArgument node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstArgumentList node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstBlock node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCaseBlock node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstLogicalOrExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstLogicalAndExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstStringConcatenateExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstBitwiseOrExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstBitwiseAndExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstEqualExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstNotEqualExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstLessThanExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstGreaterThanExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstLessEqualExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstGreaterEqualExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstAdditionExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstSubtractionExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstMultiplyingExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstDivisionExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstModuloExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstUnaryMinusExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstUnaryNotExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstIntLiteral node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstRealLiteral node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstStringLiteral node )
        => VisitChildren( node );

    public IAstNode Visit( AstIntExpression node )
        => VisitChildren( node );

    public IAstNode Visit( AstRealExpression node )
        => VisitChildren( node );

    public IAstNode Visit( AstStringExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstExpressionList node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstAssignmentExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstAssignmentExpressionList node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstSymbolExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstArrayElementExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCallExpression node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstKspPreprocessorDefine node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstKspPreprocessorUndefine node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstKspPreprocessorIfdefine node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstKspPreprocessorIfnotDefine node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstIfStatement node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstWhileStatement node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstSelectStatement node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstCallKspUserFunctionStatement node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstVariableDeclaration node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstVariableInitializer node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstPrimitiveInitializer node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstArrayInitializer node )
        => VisitChildren( node );

    public virtual IAstNode Visit( AstContinueStatement node )
        => VisitChildren( node );
}
