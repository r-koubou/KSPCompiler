using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements
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
            set => size = value;
        }

        /// <summary>
        /// Array element initialization
        /// </summary>
        public virtual AstExpressionListNode Initializer
        {
            get => initializer;
            set => initializer = value;
        }

        /// <summary>
        /// Has assign operator (`:=`)
        /// </summary>
        /// <remarks>
        /// Used to determine if it is a UI array variable during semantic analysis.
        /// </remarks>
        public virtual bool HasAssignOperator { get; set; }

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
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren( IAstVisitor visitor )
        {
            Size.Accept( visitor );
            Initializer.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
