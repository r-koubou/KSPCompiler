#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing an array element reference expression ( Left: AstSymbolExpression, Right: AstExpressionSyntaxNode )
    /// </summary>
    public class AstArrayElement : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayElement( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.ArrayElement, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayElement( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.ArrayElement, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayElement()
            : base( AstNodeId.ArrayElement, null, null, null )
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
