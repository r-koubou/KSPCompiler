using System;

using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// Ast node representing a primitive variable initialization
    /// </summary>
    public class AstPrimitiveInitializerNode : AstInitializerNode
    {
        /// <summary>
        /// Assignment expression
        /// </summary>
        public AstExpressionNode Expression { get; }

        /// <summary>
        /// Assignment multiple expression (ui_type, constructor)
        /// </summary>
        public AstExpressionListNode ExpressionList { get; }

        public AstPrimitiveInitializerNode()
            : base( AstNodeId.PrimitiveInitializer, NullAstNode.Instance )
        {
            Expression     = NullAstExpressionNode.Instance;
            ExpressionList = new AstExpressionListNode( this );
        }

        public AstPrimitiveInitializerNode(
            IAstNode parent,
            AstExpressionNode expression,
            AstExpressionListNode expressionList )
            : base( AstNodeId.PrimitiveInitializer, parent )
        {
            Expression     = expression;
            ExpressionList = expressionList;
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
