using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Nodes
{
    public interface IAstVisitor<out T>
    {
        public T Visit( NullAstNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( NullAstExpressionSyntaxNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( NullAstInitializer node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstCompilationUnit node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstCallbackDeclaration node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstUserFunctionDeclaration node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstArgument node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstArgumentList node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstBlock node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstCaseBlock node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstLogicalOrExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstLogicalAndExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstLogicalXorExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstStringConcatenateExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstBitwiseOrExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstBitwiseAndExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstBitwiseXorExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstEqualExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstNotEqualExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstLessThanExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstGreaterThanExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstLessEqualExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstGreaterEqualExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstAdditionExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstSubtractionExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstMultiplyingExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstDivisionExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstModuloExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstUnaryMinusExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstUnaryNotExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstUnaryLogicalNotExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstIntLiteral node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstRealLiteral node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstStringLiteral node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstExpressionList node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstAssignmentExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstAssignmentExpressionList node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstDefaultExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstArrayElementExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstCallExpression node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstKspPreprocessorDefine node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstKspPreprocessorUndefine node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstKspPreprocessorIfdefine node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstKspPreprocessorIfnotDefine node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstIfStatement node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstWhileStatement node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstSelectStatement node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstCallKspUserFunctionStatement node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstContinueStatement node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstVariableDeclaration node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstVariableInitializer node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstPrimitiveInitializer node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstArrayInitializer node, AbortTraverseToken abortTraverseToken );
    }

    /// <summary>
    /// Non-generic version of <see cref="IAstVisitor{T}"/>. <see cref="IAstNode" /> is used as the return type.
    /// </summary>
    public interface IAstVisitor : IAstVisitor<IAstNode> {}
}
