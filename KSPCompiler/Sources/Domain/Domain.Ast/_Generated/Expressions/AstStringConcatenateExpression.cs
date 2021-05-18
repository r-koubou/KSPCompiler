// Generated by CodeGenerators/ASTCodeGenerator

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing a string concatenation operator (&).
    /// </summary>
    public partial class AstStringConcatenateExpression : AstExpressionSyntaxNode
    {

        /// <summary>
        /// Ctor.
        /// </summary>
        public AstStringConcatenateExpression(
            IAstNode parent,
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base( AstNodeId.StringConcatenateExpression, parent, left, right )
        {}

        public AstStringConcatenateExpression(
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base( AstNodeId.StringConcatenateExpression, IAstNode.None, left, right )
        {}


    }
}
