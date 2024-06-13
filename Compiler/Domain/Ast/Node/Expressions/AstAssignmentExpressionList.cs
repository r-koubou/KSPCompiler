namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing a comma-separated list of expressions (including assignment expressions).
    /// </summary>
    public class AstAssignmentExpressionList : AstNode
    {
        /// <summary>
        /// expression list
        /// </summary>
        public AstNodeList<AstAssignmentExpression> Expressions { get; }

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
        public AstAssignmentExpressionList( IAstNode parent )
            : base( AstNodeId.AssignmentExpressionList, parent )
        {
            Expressions = new AstNodeList<AstAssignmentExpression>( this );
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignmentExpressionList()
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
