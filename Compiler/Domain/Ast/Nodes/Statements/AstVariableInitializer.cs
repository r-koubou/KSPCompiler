namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a variable initialization
    /// </summary>
    public class AstVariableInitializer : AstInitializer
    {
        /// <summary>
        /// primitive variable initialization
        /// </summary>
        public AstInitializer PrimitiveInitializer { get; set; }

        /// <summary>
        /// array variable initialization
        /// </summary>
        public AstInitializer ArrayInitializer { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableInitializer()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableInitializer( IAstNode parent )
            : base( AstNodeId.VariableInitializer, parent )
        {
            PrimitiveInitializer = NullAstInitializer.Instance;
            ArrayInitializer     = NullAstInitializer.Instance;
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
            PrimitiveInitializer.AcceptChildren( visitor );
            ArrayInitializer.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}