namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions
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
