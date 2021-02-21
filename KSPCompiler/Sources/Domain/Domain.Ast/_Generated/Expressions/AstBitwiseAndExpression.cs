// Generated by CodeGenerators/ASTCodeGenerator/ast_gen.py
namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// Ast node representing a bitwise AND operator
    /// </summary>
    public partial class AstBitwiseAndExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseAndExpression(
            IAstNode parent,
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base(
                AstNodeId.BitwiseAndExpression,
                parent,
                left, right )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseAndExpression(
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base(
                AstNodeId.BitwiseAndExpression,
                IAstNode.None,
                left, right )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBitwiseAndExpression()
            : base(
                AstNodeId.BitwiseAndExpression,
                IAstNode.None,
                AstExpressionSyntaxNode.None,
                AstExpressionSyntaxNode.None )
        {}
    }
}