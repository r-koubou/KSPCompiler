namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing a base node representing of a statement.
    /// </summary>
    public abstract class AstStatementSyntaxNode : AstNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected AstStatementSyntaxNode(
            AstNodeId id,
            IAstNode parent )
            : base( id, parent )
        {}

        #region IASTNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
        {
            return visitor.Visit( this );
        }
        #endregion
    }
}
