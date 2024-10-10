namespace KSPCompiler.Domain.Ast.Nodes.Blocks
{
    /// <summary>
    /// AST node representing an argument list
    /// </summary>
    public class AstArgumentListNode : AstNode
    {
        /// <summary>
        /// Argument List
        /// </summary>
        public AstNodeList<AstArgumentNode> Arguments { get; }

        public bool HasArgument
            => Arguments is { Count: > 0 };

        public int ArgumentCount
            => HasArgument ? Arguments.Count : 0;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgumentListNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgumentListNode( IAstNode parent )
            : base( AstNodeId.ArgumentList, parent )
        {
            Arguments = new AstNodeList<AstArgumentNode>( this );
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 1;

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
            // Do nothing
        }

        #endregion IAstNodeAcceptor
    }
}
