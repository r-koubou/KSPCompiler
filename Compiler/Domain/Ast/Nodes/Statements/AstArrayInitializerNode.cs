using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing an array variable initialization
    /// </summary>
    public class AstArrayInitializerNode : AstStatementNode
    {
        /// <summary>
        /// Number of array elements
        /// </summary>
        public AstExpressionNode Size { get; set; }

        /// <summary>
        /// Array element initialization
        /// </summary>
        public AstExpressionListNode Initializer { get; set; }

        /// <summary>
        /// UI type initialization when variable is a UI type
        /// </summary>
        public AstExpressionListNode UITypeInitializer { get; }

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
            Size              = NullAstExpressionNode.Instance;
            Initializer       = new AstExpressionListNode( this );
            UITypeInitializer = new AstExpressionListNode( this );
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            // array size
            // array initializer
            // ui type initializer
            => 3;

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
