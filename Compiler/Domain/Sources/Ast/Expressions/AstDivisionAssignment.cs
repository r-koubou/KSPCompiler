#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing an assignment (/=)
    /// </summary>
    public class AstDivisionAssignment : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionAssignment( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.DivisionAssignment, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionAssignment( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.DivisionAssignment, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionAssignment()
            : base( AstNodeId.DivisionAssignment, null, null, null )
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
