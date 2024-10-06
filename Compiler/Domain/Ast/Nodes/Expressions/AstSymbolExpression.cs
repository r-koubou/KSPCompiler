namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a symbolic reference
    /// </summary>
    public class AstSymbolExpression : AstExpressionSyntaxNode, INameable
    {
        /// <summary>
        /// Represents whether the node is symbol(variable, commands, callback, etc.) and its reserved (built-in) by NI.
        /// </summary>
        public virtual bool Reserved { get; set; }

        #region INameable

        ///
        /// <inheritdoc/>
        ///
        public string Name { get; set; }  = string.Empty;

        #endregion INameable

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSymbolExpression()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSymbolExpression( IAstNode parent )
            : base( AstNodeId.Symbol, parent ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSymbolExpression( AstNodeId id, IAstNode parent )
            : base( id, parent ) {}

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
