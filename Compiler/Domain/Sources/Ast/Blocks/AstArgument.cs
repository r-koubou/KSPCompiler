#nullable disable

namespace KSPCompiler.Domain.Ast.Blocks
{
    /// <summary>
    /// AST node representing an argument
    /// </summary>
    public class AstArgument : AstFunctionalSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgument( IAstNode parent = null )
            : base( AstNodeId.Argument, parent )
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
