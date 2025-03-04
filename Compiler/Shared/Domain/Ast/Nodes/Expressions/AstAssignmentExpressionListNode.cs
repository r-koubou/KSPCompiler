namespace KSPCompiler.Shared.Domain.Ast.Nodes.Expressions
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
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren( IAstVisitor visitor )
        {
            foreach( var n in Expressions )
            {
                n.Accept( visitor );
            }
        }

        #endregion IAstNodeAcceptor
    }
}
