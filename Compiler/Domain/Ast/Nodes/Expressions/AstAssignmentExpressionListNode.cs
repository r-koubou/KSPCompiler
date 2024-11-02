namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a comma-separated list of expressions (including assignment expressions).
    /// </summary>
    /// <remarks>
    /// KSP standard syntax DOES NOT SUPPORTED. THIS IS A CUSTOM EXTENSION FOR FUTURE USE.
    /// </remarks>
    public class AstAssignmentExpressionListNode : AstNode
    {
        /// <summary>
        /// expression list
        /// </summary>
        public AstNodeList<AstAssignStatementNode> Expressions { get; }

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
            Expressions = new AstNodeList<AstAssignStatementNode>( this );
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
