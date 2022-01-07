#nullable disable

using System;

using KSPCompiler.Domain.Ast.Expressions;

namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing an array variable initialization
    /// </summary>
    public class AstArrayInitializer : AstStatementSyntaxNode
    {
        /// <summary>
        /// Number of array elements
        /// </summary>
        public AstExpressionSyntaxNode Size { get; set; }

        /// <summary>
        /// Array element initialization
        /// </summary>
        public AstExpressionList Initializer { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayInitializer( IAstNode parent = null )
            : base( AstNodeId.ArrayInitializer, parent )
        {
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
