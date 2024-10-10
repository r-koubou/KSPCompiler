using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Nodes
{
    public interface IAstVisitor<out T>
    {
        public T Visit( NullAstNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( NullAstExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( NullAstInitializerNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstCompilationUnitNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstCallbackDeclarationNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstUserFunctionDeclarationNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstArgumentNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstArgumentListNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstBlockNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstCaseBlock node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstLogicalOrExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstLogicalAndExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstLogicalXorExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstStringConcatenateExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstBitwiseOrExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstBitwiseAndExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstBitwiseXorExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstEqualExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstNotEqualExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstLessThanExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstGreaterThanExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstLessEqualExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstGreaterEqualExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstAdditionExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstSubtractionExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstMultiplyingExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstDivisionExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstModuloExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstUnaryMinusExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstUnaryNotExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstUnaryLogicalNotExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstIntLiteralNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstRealLiteralNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstStringLiteralNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstExpressionListNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstAssignmentExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstAssignmentExpressionListNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstDefaultExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstArrayElementExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstCallCommandExpressionNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstKspPreprocessorDefineNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstKspPreprocessorUndefineNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstKspPreprocessorIfdefineNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstKspPreprocessorIfnotDefineNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstIfStatementNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstWhileStatementNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstSelectStatementNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstCallKspUserFunctionStatementNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstContinueStatementNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstVariableDeclarationNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstVariableInitializerNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstPrimitiveInitializerNode node, AbortTraverseToken abortTraverseToken );
        public T Visit( AstArrayInitializerNode node, AbortTraverseToken abortTraverseToken );
    }

    /// <summary>
    /// Non-generic version of <see cref="IAstVisitor{T}"/>. <see cref="IAstNode" /> is used as the return type.
    /// </summary>
    public interface IAstVisitor : IAstVisitor<IAstNode> {}
}
