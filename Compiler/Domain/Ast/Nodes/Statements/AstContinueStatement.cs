namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing an continue statement
    /// </summary>
    public class AstContinueStatement : AstStatementSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstContinueStatement()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstContinueStatement( IAstNode parent )
            : base( AstNodeId.ContinueStatement, parent )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            throw new System.NotImplementedException();
        }

        #endregion IAstNodeAcceptor
    }
}
