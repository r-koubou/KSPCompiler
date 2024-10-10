namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a variable initialization
    /// </summary>
    public class AstVariableInitializerNode : AstInitializerNode
    {
        /// <summary>
        /// primitive variable initialization
        /// </summary>
        public AstInitializerNode PrimitiveInitializer { get; set; }

        /// <summary>
        /// array variable initialization
        /// </summary>
        public AstInitializerNode ArrayInitializer { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableInitializerNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableInitializerNode( IAstNode parent )
            : base( AstNodeId.VariableInitializer, parent )
        {
            PrimitiveInitializer = NullAstInitializerNode.Instance;
            ArrayInitializer     = NullAstInitializerNode.Instance;
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

                if( PrimitiveInitializer != NullAstInitializerNode.Instance )
                {
                    result++;
                }
                if( ArrayInitializer != NullAstInitializerNode.Instance )
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
