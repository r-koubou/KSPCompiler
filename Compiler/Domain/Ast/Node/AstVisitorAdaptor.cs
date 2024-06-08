using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Expressions;
using KSPCompiler.Domain.Ast.Node.Statements;

namespace KSPCompiler.Domain.Ast.Node;

public abstract class AstVisitorAdaptor<T> : IAstVisitor<T>
{
    public virtual T Visit( AstCompilationUnit node )
        => node.Accept( this );

    public virtual T Visit( AstCallbackDeclaration node )
        => node.Accept( this );

    public virtual T Visit( AstUserFunctionDeclaration node )
        => node.Accept( this );

    public virtual T Visit( AstArgument node )
        => node.Accept( this );

    public virtual T Visit( AstArgumentList node )
        => node.Accept( this );

    public virtual T Visit( AstBlock node )
        => node.Accept( this );

    public virtual T Visit( AstCaseBlock node )
        => node.Accept( this );

    public virtual T Visit( AstLogicalOrExpression node )
        => node.Accept( this );

    public virtual T Visit( AstLogicalAndExpression node )
        => node.Accept( this );

    public virtual T Visit( AstStringConcatenateExpression node )
        => node.Accept( this );

    public virtual T Visit( AstBitwiseOrExpression node )
        => node.Accept( this );

    public virtual T Visit( AstBitwiseAndExpression node )
        => node.Accept( this );

    public virtual T Visit( AstEqualExpression node )
        => node.Accept( this );

    public virtual T Visit( AstNotEqualExpression node )
        => node.Accept( this );

    public virtual T Visit( AstLessThanExpression node )
        => node.Accept( this );

    public virtual T Visit( AstGreaterThanExpression node )
        => node.Accept( this );

    public virtual T Visit( AstLessEqualExpression node )
        => node.Accept( this );

    public virtual T Visit( AstGreaterEqualExpression node )
        => node.Accept( this );

    public virtual T Visit( AstAdditionExpression node )
        => node.Accept( this );

    public virtual T Visit( AstSubtractionExpression node )
        => node.Accept( this );

    public virtual T Visit( AstMultiplyingExpression node )
        => node.Accept( this );

    public virtual T Visit( AstDivisionExpression node )
        => node.Accept( this );

    public virtual T Visit( AstModuloExpression node )
        => node.Accept( this );

    public virtual T Visit( AstUnaryMinusExpression node )
        => node.Accept( this );

    public virtual T Visit( AstUnaryNotExpression node )
        => node.Accept( this );

    public virtual T Visit( AstIntLiteral node )
        => node.Accept( this );

    public virtual T Visit( AstRealLiteral node )
        => node.Accept( this );

    public virtual T Visit( AstStringLiteral node )
        => node.Accept( this );

    public virtual T Visit( AstExpressionList node )
        => node.Accept( this );

    public virtual T Visit( AstAssignmentExpression node )
        => node.Accept( this );

    public virtual T Visit( AstAssignmentExpressionList node )
        => node.Accept( this );

    public virtual T Visit( AstSymbolExpression node )
        => node.Accept( this );

    public virtual T Visit( AstArrayElementExpression node )
        => node.Accept( this );

    public virtual T Visit( AstCallExpression node )
        => node.Accept( this );

    public virtual T Visit( AstKspPreprocessorDefine node )
        => node.Accept( this );

    public virtual T Visit( AstKspPreprocessorUndefine node )
        => node.Accept( this );

    public virtual T Visit( AstKspPreprocessorIfdefine node )
        => node.Accept( this );

    public virtual T Visit( AstKspPreprocessorIfnotDefine node )
        => node.Accept( this );

    public virtual T Visit( AstIfStatement node )
        => node.Accept( this );

    public virtual T Visit( AstWhileStatement node )
        => node.Accept( this );

    public virtual T Visit( AstSelectStatement node )
        => node.Accept( this );

    public virtual T Visit( AstCallKspUserFunctionStatement node )
        => node.Accept( this );

    public virtual T Visit( AstVariableDeclaration node )
        => node.Accept( this );

    public virtual T Visit( AstVariableInitializer node )
        => node.Accept( this );

    public virtual T Visit( AstPrimitiveInitializer node )
        => node.Accept( this );

    public virtual T Visit( AstArrayInitializer node )
        => node.Accept( this );

    public T Visit( AstContinueStatement node )
        => node.Accept( this );
}
