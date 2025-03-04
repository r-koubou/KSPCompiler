using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Shared.Domain.Ast.Nodes.Statements
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
        public AstExpressionListNode UIInitializer { get; }

        public AstPrimitiveInitializerNode()
            : base( AstNodeId.PrimitiveInitializer, NullAstNode.Instance )
        {
            Expression    = NullAstExpressionNode.Instance;
            UIInitializer = new AstExpressionListNode( this );
        }

        public AstPrimitiveInitializerNode(
            IAstNode parent,
            AstExpressionNode expression,
            AstExpressionListNode uiInitializer )
            : base( AstNodeId.PrimitiveInitializer, parent )
        {
            Expression    = expression;
            UIInitializer = uiInitializer;
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
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren( IAstVisitor visitor )
        {
            Expression.Accept( visitor );
            UIInitializer.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
