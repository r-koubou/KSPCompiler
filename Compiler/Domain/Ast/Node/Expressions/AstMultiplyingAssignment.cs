using System;

namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing an assignment (*=)
    /// </summary>
    public class AstMultiplyingAssignment : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingAssignment( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.MultiplyingAssignment, parent, left, right )
        {
            throw new NotImplementedException( "`*=` is not KSP Syntax (Tt will be extended syntax in the future)" );
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingAssignment( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.MultiplyingAssignment, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstMultiplyingAssignment()
            : base( AstNodeId.MultiplyingAssignment, null )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
