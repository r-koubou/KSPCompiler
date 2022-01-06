#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing a floating-point literal
    /// </summary>
    public class AstRealLiteral : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstRealLiteral( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.RealLiteral, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstRealLiteral( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.RealLiteral, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstRealLiteral()
            : base( AstNodeId.RealLiteral, null, null, null )
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
