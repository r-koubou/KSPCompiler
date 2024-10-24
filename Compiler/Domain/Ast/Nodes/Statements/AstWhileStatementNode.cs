namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing an while statement
    /// </summary>
    public class AstWhileStatementNode : AstConditionalStatementNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstWhileStatementNode()
            : this( NullAstNode.Instance )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstWhileStatementNode( IAstNode parent )
            : base( AstNodeId.WhileStatement, parent )
        {
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 2; // condition & code block

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
