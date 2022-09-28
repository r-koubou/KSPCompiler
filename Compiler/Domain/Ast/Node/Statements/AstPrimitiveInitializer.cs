using System;

using KSPCompiler.Domain.Ast.Node.Expressions;

namespace KSPCompiler.Domain.Ast.Node.Statements
{
    /// <summary>
    /// Ast node representing a primitive variable initialization
    /// </summary>
    public class AstPrimitiveInitializer : AstStatementSyntaxNode
    {
        /// <summary>
        /// Assignment expression
        /// </summary>
        public AstExpressionSyntaxNode? Expression { get; }

        /// <summary>
        /// Assignment multiple expression (ui_type, constructor)
        /// </summary>
        public AstExpressionList? ExpressionList { get; }

        public AstPrimitiveInitializer(
            IAstNode? parent = null,
            AstExpressionSyntaxNode? expression = null,
            AstExpressionList? expressionList = null )
            : base( AstNodeId.PrimitiveInitializer, parent )
        {
            Expression     = expression;
            ExpressionList = expressionList;
        }

        #region IAstNodeAcceptor

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
            throw new NotImplementedException();
        }

        #endregion IAstNodeAcceptor
    }
}
