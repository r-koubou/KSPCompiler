using System;

using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// Ast node representing a primitive variable initialization
    /// </summary>
    public class AstPrimitiveInitializer : AstInitializer
    {
        /// <summary>
        /// Assignment expression
        /// </summary>
        public AstExpressionSyntaxNode Expression { get; }

        /// <summary>
        /// Assignment multiple expression (ui_type, constructor)
        /// </summary>
        public AstExpressionList ExpressionList { get; }

        public AstPrimitiveInitializer()
            : base( AstNodeId.PrimitiveInitializer, NullAstNode.Instance )
        {
            Expression     = NullAstExpressionSyntaxNode.Instance;
            ExpressionList = new AstExpressionList( this );
        }

        public AstPrimitiveInitializer(
            IAstNode parent,
            AstExpressionSyntaxNode expression,
            AstExpressionList expressionList )
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
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            throw new NotImplementedException();
        }

        #endregion IAstNodeAcceptor
    }
}
