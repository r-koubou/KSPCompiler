#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing a modulo operator (%)
    /// </summary>
    public class AstModuloExpression : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstModuloExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Modulo, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstModuloExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Modulo, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstModuloExpression()
            : base( AstNodeId.Modulo, null, null, null )
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
