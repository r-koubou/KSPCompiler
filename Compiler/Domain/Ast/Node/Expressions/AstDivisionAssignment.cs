using System;

namespace KSPCompiler.Domain.Ast.Node.Expressions
{
    /// <summary>
    /// AST node representing an assignment (/=)
    /// </summary>
    public class AstDivisionAssignment : AstExpressionSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionAssignment( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.DivisionAssignment, parent, left, right )
        {
            throw new NotImplementedException( "`/=` is not KSP Syntax (Tt will be extended syntax in the future)" );
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionAssignment( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.DivisionAssignment, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDivisionAssignment()
            : base( AstNodeId.DivisionAssignment, null )
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
