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
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
        {
            get
            {
                var result = 0;

                if( PrimitiveInitializer != NullAstInitializer.Instance )
                {
                    result++;
                }
                if( ArrayInitializer != NullAstInitializer.Instance )
                {
                    result++;
                }

                return result;
            }
        }

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
            PrimitiveInitializer.AcceptChildren( visitor, abortTraverseToken );

            if( abortTraverseToken.Aborted )
            {
                return;
            }

            ArrayInitializer.AcceptChildren( visitor, abortTraverseToken );
        }

        #endregion IAstNodeAcceptor
    }
}
