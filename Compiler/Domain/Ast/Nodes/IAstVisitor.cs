using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Nodes
{
    public interface IAstVisitor<out T>
    {
        public T Visit( NullAstNode node );
        public T Visit( NullAstExpressionNode node );
        public T Visit( NullAstInitializerNode node );
        public T Visit( AstCompilationUnitNode node );
        public T Visit( AstCallbackDeclarationNode node );
        public T Visit( AstUserFunctionDeclarationNode node );
        public T Visit( AstArgumentNode node );
        public T Visit( AstArgumentListNode node );
        public T Visit( AstBlockNode node );
        public T Visit( AstCaseBlock node );
        public T Visit( AstLogicalOrExpressionNode node );
        public T Visit( AstLogicalAndExpressionNode node );
        public T Visit( AstLogicalXorExpressionNode node );
        public T Visit( AstStringConcatenateExpressionNode node );
        public T Visit( AstBitwiseOrExpressionNode node );
        public T Visit( AstBitwiseAndExpressionNode node );
        public T Visit( AstBitwiseXorExpressionNode node );
        public T Visit( AstEqualExpressionNode node );
        public T Visit( AstNotEqualExpressionNode node );
        public T Visit( AstLessThanExpressionNode node );
        public T Visit( AstGreaterThanExpressionNode node );
        public T Visit( AstLessEqualExpressionNode node );
        public T Visit( AstGreaterEqualExpressionNode node );
        public T Visit( AstAdditionExpressionNode node );
        public T Visit( AstSubtractionExpressionNode node );
        public T Visit( AstMultiplyingExpressionNode node );
        public T Visit( AstDivisionExpressionNode node );
        public T Visit( AstModuloExpressionNode node );
        public T Visit( AstUnaryMinusExpressionNode node );
        public T Visit( AstUnaryNotExpressionNode node );
        public T Visit( AstUnaryLogicalNotExpressionNode node );
        public T Visit( AstIntLiteralNode node );
        public T Visit( AstRealLiteralNode node );
        public T Visit( AstStringLiteralNode node );
        public T Visit( AstExpressionListNode node );
        public T Visit( AstAssignmentExpressionNode node );
        public T Visit( AstAssignmentExpressionListNode node );
        public T Visit( AstDefaultExpressionNode node );
        public T Visit( AstSymbolExpressionNode node );
        public T Visit( AstArrayElementExpressionNode node );
        public T Visit( AstCallCommandExpressionNode node );
        public T Visit( AstKspPreprocessorDefineNode node );
        public T Visit( AstKspPreprocessorUndefineNode node );
        public T Visit( AstKspPreprocessorIfdefineNode node );
        public T Visit( AstKspPreprocessorIfnotDefineNode node );
        public T Visit( AstIfStatementNode node );
        public T Visit( AstWhileStatementNode node );
        public T Visit( AstSelectStatementNode node );
        public T Visit( AstCallKspUserFunctionStatementNode node );
        public T Visit( AstContinueStatementNode node );
        public T Visit( AstVariableDeclarationNode node );
        public T Visit( AstVariableInitializerNode node );
        public T Visit( AstPrimitiveInitializerNode node );
        public T Visit( AstArrayInitializerNode node );
    }

    /// <summary>
    /// Non-generic version of <see cref="IAstVisitor{T}"/>. <see cref="IAstNode" /> is used as the return type.
    /// </summary>
    public interface IAstVisitor : IAstVisitor<IAstNode> {}
}
