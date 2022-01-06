#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing a multiplying operator (*)
    /// </summary>
    public class AstMultiplyingExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Multiplying, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Multiplying, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingExpression()
            : base( AstNodeId.Multiplying, null, null, null )
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
