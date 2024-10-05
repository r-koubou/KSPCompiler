namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing a comma-separated list of expressions
    /// </summary>
    public class AstExpressionList : AstExpressionSyntaxNode
    {
        /// <summary>
        /// expression list
        /// </summary>
        public AstNodeList<AstExpressionSyntaxNode> Expressions { get; }

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
        public AstExpressionList( IAstNode parent )
            : base( AstNodeId.ExpressionList, parent )
        {
            Expressions = new AstNodeList<AstExpressionSyntaxNode>( this );
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionList()
            : this( NullAstNode.Instance ) {}

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            foreach( var n in Expressions )
            {
                n.Accept( visitor );
            }
        }

        #endregion IAstNodeAcceptor
    }
}
