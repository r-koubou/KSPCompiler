namespace KSPCompiler.Domain.Ast.Nodes.Blocks
{
    /// <summary>
    /// AST node representing a callback, block in function
    /// </summary>
    public class AstBlock : AstNode
    {
        /// <summary>
        /// Statements
        /// </summary>
        public AstNodeList<AstNode> Statements { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBlock() : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBlock( IAstNode parent )
            : base( AstNodeId.Block, parent )
        {
            Statements = new AstNodeList<AstNode>( this );
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this, abortTraverseToken );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            if( abortTraverseToken.Aborted )
            {
                return;
            }

            foreach( var n in Statements )
            {
                n.Accept( visitor, abortTraverseToken );

                if( abortTraverseToken.Aborted )
                {
                    return;
                }
            }
        }

        #endregion IAstNodeAcceptor
    }
}
