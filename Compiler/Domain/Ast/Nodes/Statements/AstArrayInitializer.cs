using System;

using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing an array variable initialization
    /// </summary>
    public class AstArrayInitializer : AstInitializer
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
        public AstArrayInitializer()
            : this( NullAstNode.Instance) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayInitializer( IAstNode parent )
            : base( AstNodeId.ArrayInitializer, parent )
        {
            Size        = NullAstExpressionSyntaxNode.Instance;
            Initializer = new AstExpressionList( this );
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