#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing an invoking the KSP command
    /// </summary>
    public class AstCall : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstCall( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Call, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCall( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.Call, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCall()
            : base( AstNodeId.Call, null, null, null )
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
