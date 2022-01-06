#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing an unary operator: NOT
    /// </summary>
    public class AstUnaryNotExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryNotExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.UnaryNot, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryNotExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.UnaryNot, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUnaryNotExpression()
            : base( AstNodeId.UnaryNot, null, null, null )
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
