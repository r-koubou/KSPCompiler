using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing an array variable initialization
    /// </summary>
    public class AstArrayInitializerNode : AstStatementNode
    {
        private AstExpressionNode size;
        private AstExpressionListNode initializer;

        /// <summary>
        /// Number of array elements
        /// </summary>
        public virtual AstExpressionNode Size
        {
            get => size;
            set => size = value ?? NullAstExpressionNode.Instance;
        }

        /// <summary>
        /// Array element initialization
        /// </summary>
        public virtual AstExpressionListNode Initializer
        {
            get => initializer;
            set => initializer = value ?? new AstExpressionListNode( this );
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayInitializerNode()
            : this( NullAstNode.Instance) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayInitializerNode( IAstNode parent )
            : base( AstNodeId.ArrayInitializer, parent )
        {
            size        = NullAstExpressionNode.Instance;
            initializer = new AstExpressionListNode( this );
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            // array size
            // array / ui initializer
            => 2;

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
