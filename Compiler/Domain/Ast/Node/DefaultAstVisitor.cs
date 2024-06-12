using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Expressions;
using KSPCompiler.Domain.Ast.Node.Statements;

namespace KSPCompiler.Domain.Ast.Node;

public abstract class DefaultAstVisitor : IAstVisitor<AstNode>
{
    public virtual AstNode VisitChildren( AstNode node )
    {
        node.AcceptChildren( this );
        return node;
    }

    public AstNode Visit( NullAstNode node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstCompilationUnit node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstCallbackDeclaration node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstUserFunctionDeclaration node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstArgument node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstArgumentList node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstBlock node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstCaseBlock node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstLogicalOrExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstLogicalAndExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstStringConcatenateExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstBitwiseOrExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstBitwiseAndExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstEqualExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstNotEqualExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstLessThanExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstGreaterThanExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstLessEqualExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstGreaterEqualExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstAdditionExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstSubtractionExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstMultiplyingExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstDivisionExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstModuloExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstUnaryMinusExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstUnaryNotExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstIntLiteral node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstRealLiteral node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstStringLiteral node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstExpressionList node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstAssignmentExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstAssignmentExpressionList node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstSymbolExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstArrayElementExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstCallExpression node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstKspPreprocessorDefine node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstKspPreprocessorUndefine node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstKspPreprocessorIfdefine node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstKspPreprocessorIfnotDefine node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstIfStatement node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstWhileStatement node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstSelectStatement node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstCallKspUserFunctionStatement node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstVariableDeclaration node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstVariableInitializer node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstPrimitiveInitializer node )
        => VisitChildren( node );

    public virtual AstNode Visit( AstArrayInitializer node )
        => VisitChildren( node );

    public AstNode Visit( AstContinueStatement node )
        => VisitChildren( node );
}
