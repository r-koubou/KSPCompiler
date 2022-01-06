#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing a comma-separated list of expressions
    /// </summary>
    public class AstExpressionList : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionList( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.ExpressionList, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionList( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.ExpressionList, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionList()
            : base( AstNodeId.ExpressionList, null, null, null )
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
