// Generated by CodeGenerators/ASTCodeGenerator/ast_gen.py
namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// Ast node representing a string concatenation operator (&)
    /// </summary>
    public partial class AstStringConcatenateExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringConcatenateExpression(
            IAstNode parent,
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base(
                AstNodeId.StringConcatenateExpression,
                parent,
                left, right )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringConcatenateExpression(
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base(
                AstNodeId.StringConcatenateExpression,
                IAstNode.None,
                left, right )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringConcatenateExpression()
            : base(
                AstNodeId.StringConcatenateExpression,
                IAstNode.None,
                AstExpressionSyntaxNode.None,
                AstExpressionSyntaxNode.None )
        {}
    }
}