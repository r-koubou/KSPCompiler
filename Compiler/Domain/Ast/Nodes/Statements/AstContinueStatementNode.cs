namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing an continue statement
    /// </summary>
    public class AstContinueStatementNode : AstStatementNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstContinueStatementNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstContinueStatementNode( IAstNode parent )
            : base( AstNodeId.ContinueStatement, parent )
        {
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 0;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            throw new System.NotImplementedException();
        }

        #endregion IAstNodeAcceptor
    }
}