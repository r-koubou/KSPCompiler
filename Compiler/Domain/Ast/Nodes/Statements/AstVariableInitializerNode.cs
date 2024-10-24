namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a variable initialization
    /// </summary>
    public class AstVariableInitializerNode : AstStatementNode
    {
        /// <summary>
        /// primitive variable initialization
        /// </summary>
        public AstPrimitiveInitializerNode PrimitiveInitializer { get; set; }
            = NullAstPrimitiveInitializerNode.Instance;

        /// <summary>
        /// array variable initialization
        /// </summary>
        public AstArrayInitializerNode ArrayInitializer { get; set; }
            = NullAstArrayInitializerNode.Instance;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableInitializerNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableInitializerNode( IAstNode parent )
            : base( AstNodeId.VariableInitializer, parent ) {}

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
        {
            get
            {
                var result = 0;

                if( PrimitiveInitializer != NullAstPrimitiveInitializerNode.Instance )
                {
                    result++;
                }
                if( ArrayInitializer != NullAstArrayInitializerNode.Instance )
                {
                    result++;
                }

                return result;
            }
        }

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
