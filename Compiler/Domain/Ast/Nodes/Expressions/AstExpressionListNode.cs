namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a comma-separated list of expressions
    /// </summary>
    public class AstExpressionListNode : AstExpressionNode
    {
        /// <summary>
        /// expression list
        /// </summary>
        public AstNodeList<AstExpressionNode> Expressions { get; }

        /// <summary>
        /// Alias for <see cref="Expressions"/>.<see cref="AstNodeList{TNode}.Count"/>
        /// </summary>
        public int Count => Expressions.Count;

        /// <summary>
        /// Alias for <see cref="Expressions"/>.<see cref="AstNodeList{TNode}.Empty"/>
        /// </summary>
        public bool Empty => Expressions.Empty;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionListNode( IAstNode parent )
            : base( AstNodeId.ExpressionList, parent )
        {
            Expressions = new AstNodeList<AstExpressionNode>( this );
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionListNode()
            : this( NullAstNode.Instance ) {}

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
            => visitor.Visit( this , abortTraverseToken );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            if( abortTraverseToken.Aborted )
            {
                return;
            }

            foreach( var n in Expressions )
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
