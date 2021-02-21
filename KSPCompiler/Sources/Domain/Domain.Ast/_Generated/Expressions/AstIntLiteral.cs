// Generated by CodeGenerators/ASTCodeGenerator/ast_gen.py
namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// Ast node representing an integer literal
    /// </summary>
    public partial class AstIntLiteral : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral(
            IAstNode parent,
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base(
                AstNodeId.IntLiteral,
                parent,
                left, right )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral(
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base(
                AstNodeId.IntLiteral,
                IAstNode.None,
                left, right )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral()
            : base(
                AstNodeId.IntLiteral,
                IAstNode.None,
                AstExpressionSyntaxNode.None,
                AstExpressionSyntaxNode.None )
        {}
    }
}