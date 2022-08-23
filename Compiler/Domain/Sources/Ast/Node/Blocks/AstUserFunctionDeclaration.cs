namespace KSPCompiler.Domain.Ast.Node.Blocks
{
    /// <summary>
    /// AST node representing an user Defined Functions (KSP)
    /// </summary>
    public class AstUserFunctionDeclaration : AstFunctionalSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstUserFunctionDeclaration( IAstNode? parent = null )
            : base( AstNodeId.UserFunctionDeclaration, parent )
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
