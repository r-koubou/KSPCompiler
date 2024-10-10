namespace KSPCompiler.Domain.Ast.Nodes.Blocks
{
    /// <summary>
    /// AST node representing an user Defined Functions (KSP)
    /// </summary>
    public class AstUserFunctionDeclarationNode : AstFunctionalNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstUserFunctionDeclarationNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUserFunctionDeclarationNode( IAstNode parent )
            : base( AstNodeId.UserFunctionDeclaration, parent )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );


        #endregion IAstNodeAcceptor
    }
}
