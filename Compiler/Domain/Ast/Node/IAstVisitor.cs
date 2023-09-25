using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Expressions;
using KSPCompiler.Domain.Ast.Node.Statements;

namespace KSPCompiler.Domain.Ast.Node
{
    public interface IAstVisitor<out T>
    {
        public T Visit( AstCompilationUnit node );
        public T Visit( AstCallbackDeclaration node );
        public T Visit( AstUserFunctionDeclaration node );
        public T Visit( AstArgument node );
        public T Visit( AstArgumentList node );
        public T Visit( AstBlock node );
        public T Visit( AstCaseBlock node );
        public T Visit( AstLogicalOrExpression node );
        public T Visit( AstLogicalAndExpression node );
        public T Visit( AstStringConcatenateExpression node );
        public T Visit( AstBitwiseOrExpression node );
        public T Visit( AstBitwiseAndExpression node );
        public T Visit( AstEqualExpression node );
        public T Visit( AstNotEqualExpression node );
        public T Visit( AstLessThanExpression node );
        public T Visit( AstGreaterThanExpression node );
        public T Visit( AstLessEqualExpression node );
        public T Visit( AstGreaterEqualExpression node );
        public T Visit( AstAdditionExpression node );
        public T Visit( AstSubtractionExpression node );
        public T Visit( AstMultiplyingExpression node );
        public T Visit( AstDivisionExpression node );
        public T Visit( AstModuloExpression node );
        public T Visit( AstUnaryMinusExpression node );
        public T Visit( AstUnaryNotExpression node );
        public T Visit( AstIntLiteral node );
        public T Visit( AstRealLiteral node );
        public T Visit( AstStringLiteral node );
        public T Visit( AstExpressionList node );
        public T Visit( AstAssignmentExpression node );
        public T Visit( AstAssignmentExpressionList node );
        public T Visit( AstSymbolExpression node );
        public T Visit( AstArrayElementExpression node );
        public T Visit( AstCallExpression node );
        public T Visit( AstKspPreprocessorDefine node );
        public T Visit( AstKspPreprocessorUndefine node );
        public T Visit( AstKspPreprocessorIfdefine node );
        public T Visit( AstKspPreprocessorIfnotDefine node );
        public T Visit( AstIfStatement node );
        public T Visit( AstWhileStatement node );
        public T Visit( AstSelectStatement node );
        public T Visit( AstCallKspUserFunctionStatement node );
        public T Visit( AstVariableDeclaration node );
        public T Visit( AstVariableInitializer node );
        public T Visit( AstPrimitiveInitializer node );
        public T Visit( AstArrayInitializer node );
    }
}
