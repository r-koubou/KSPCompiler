#nullable disable

namespace ${namespace}
{
    /// <summary>
    /// AST node representing ${description}
    /// </summary>
    public class ${classname} : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.${name}, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.${name}, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}()
            : base( AstNodeId.${name}, null, null, null )
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
