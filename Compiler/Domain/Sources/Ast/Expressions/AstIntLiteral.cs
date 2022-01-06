#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing an integer literal
    /// </summary>
    public class AstIntLiteral : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.IntLiteral, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.IntLiteral, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntLiteral()
            : base( AstNodeId.IntLiteral, null, null, null )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
