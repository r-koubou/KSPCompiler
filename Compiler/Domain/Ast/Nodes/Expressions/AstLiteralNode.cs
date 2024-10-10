namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing the base class for all literal nodes
    /// </summary>
    public abstract class AstLiteralNode<TValue> : AstDefaultExpressionNode
    {
        /// <summary>
        /// literal value
        /// </summary>
        public TValue Value { get; set; }

        ///
        /// <inheritdoc/>
        ///
        public override bool IsConstant => true;

        /// <summary>
        /// Ctor
        /// </summary>
        protected AstLiteralNode( AstNodeId id, TValue value )
            : this( id, NullAstNode.Instance, value )
        {
            Value = value;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        protected AstLiteralNode( AstNodeId id, IAstNode parent, TValue value )
            : base( id, parent )
        {
            Value = value;
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override int ChildNodeCount
            => 0;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
