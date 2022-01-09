namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing an assignment (+=)
    /// </summary>
    public class AstAdditionAssignment : AstExpressionSyntaxNode
    {
        /// <summary>q
        /// Ctor
        /// </summary>
        public AstAdditionAssignment( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.AdditionAssignment, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAdditionAssignment( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.AdditionAssignment, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAdditionAssignment()
            : base( AstNodeId.AdditionAssignment, null )
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
