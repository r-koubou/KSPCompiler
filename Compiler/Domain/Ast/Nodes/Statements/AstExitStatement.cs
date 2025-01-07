namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing an exit statement
    /// </summary>
    public class AstExitStatementNode : AstStatementNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstExitStatementNode() : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExitStatementNode( IAstNode parent )
            : base( AstNodeId.ExitStatement, parent ) {}

        #region IAstNodeAcceptor
        public override int ChildNodeCount
            => 0;

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren( IAstVisitor visitor ) {}
        #endregion IAstNodeAcceptor
    }
}
