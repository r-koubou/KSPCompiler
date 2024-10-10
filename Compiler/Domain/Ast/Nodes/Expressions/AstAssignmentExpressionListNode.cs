namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a comma-separated list of expressions (including assignment expressions).
    /// </summary>
    public class AstAssignmentExpressionListNode : AstNode
    {
        /// <summary>
        /// expression list
        /// </summary>
        public AstNodeList<AstAssignmentExpressionNode> Expressions { get; }

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
        public AstAssignmentExpressionListNode( IAstNode parent )
            : base( AstNodeId.AssignmentExpressionList, parent )
        {
            Expressions = new AstNodeList<AstAssignmentExpressionNode>( this );
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignmentExpressionListNode()
            : this( NullAstNode.Instance ) {}

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => Expressions.Count;

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
