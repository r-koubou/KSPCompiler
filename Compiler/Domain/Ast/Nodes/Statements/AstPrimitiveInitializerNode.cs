using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// Ast node representing a primitive variable initialization
    /// </summary>
    public class AstPrimitiveInitializerNode : AstStatementNode
    {
        /// <summary>
        /// Assignment expression
        /// </summary>
        public AstExpressionNode Expression { get; set; }

        /// <summary>
        /// UI type initialization when variable is a UI type
        /// </summary>
        public AstExpressionListNode UITypeInitializer { get; }

        public AstPrimitiveInitializerNode()
            : base( AstNodeId.PrimitiveInitializer, NullAstNode.Instance )
        {
            Expression    = NullAstExpressionNode.Instance;
            UITypeInitializer = new AstExpressionListNode( this );
        }

        public AstPrimitiveInitializerNode(
            IAstNode parent,
            AstExpressionNode expression,
            AstExpressionListNode uiTypeInitializer )
            : base( AstNodeId.PrimitiveInitializer, parent )
        {
            Expression        = expression;
            UITypeInitializer = uiTypeInitializer;
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
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
