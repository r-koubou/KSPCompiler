namespace KSPCompiler.Domain.Ast.Node.Blocks
{
    /// <summary>
    /// AST node representing an argument list
    /// </summary>
    public class AstArgumentList : AstNode
    {
        /// <summary>
        /// Argument List
        /// </summary>
        public AstNodeList<AstArgument> Arguments { get; }

        public bool HasArgument
            => Arguments is { Count: > 0 };

        public int ArgumentCount
            => HasArgument ? Arguments.Count : 0;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <remarks>
        /// Parent will be set to <see cref="NullAstNode.Instance"/>.
        /// </remarks>
        public AstArgumentList()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgumentList( IAstNode parent )
            : base( AstNodeId.ArgumentList, parent )
        {
            Arguments = new AstNodeList<AstArgument>( this );
        }

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
            // Do nothing
        }

        #endregion IAstNodeAcceptor
    }
}
