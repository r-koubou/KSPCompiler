#nullable disable

namespace KSPCompiler.Domain.Ast.Blocks
{
    /// <summary>
    /// AST node representing a callback definition
    /// </summary>
    public class AstCallbackDeclaration : AstFunctionalSyntaxNode, IArgumentList
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallbackDeclaration( IAstNode parent = null )
            : base( AstNodeId.CallbackDeclaration, parent )
        {
            ArgumentList = new AstArgumentList( this );
        }

        #region IArgument

        ///
        /// <inheritdoc />
        ///
        public AstArgumentList ArgumentList { get; set; }

        #endregion IArgument

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );


        #endregion IAstNodeAcceptor
    }
}
