// Generated by CodeGenerators/ASTCodeGenerator/ast_gen.py
namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// Ast node representing a multiplying operator (*)
    /// </summary>
    public partial class AstMultiplyingExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingExpression(
            IAstNode parent,
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base(
                AstNodeId.MultiplyingExpression,
                parent,
                left, right )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingExpression(
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base(
                AstNodeId.MultiplyingExpression,
                IAstNode.None,
                left, right )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingExpression()
            : base(
                AstNodeId.MultiplyingExpression,
                IAstNode.None,
                AstExpressionSyntaxNode.None,
                AstExpressionSyntaxNode.None )
        {}
    }
}