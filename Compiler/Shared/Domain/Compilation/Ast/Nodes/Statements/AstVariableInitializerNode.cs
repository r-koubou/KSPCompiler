namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a variable initialization
    /// </summary>
    public class AstVariableInitializerNode : AstStatementNode
    {
        /// <summary>
        /// primitive variable initialization
        /// </summary>
        public virtual AstPrimitiveInitializerNode PrimitiveInitializer { get; set; }
            = NullAstPrimitiveInitializerNode.Instance;

        /// <summary>
        /// array variable initialization
        /// </summary>
        public virtual AstArrayInitializerNode ArrayInitializer { get; set; }
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
            => 1;
            // PrimitiveInitializer or ArrayInitializer (exclusive)

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

            PrimitiveInitializer.AcceptChildren( visitor );
            ArrayInitializer.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
