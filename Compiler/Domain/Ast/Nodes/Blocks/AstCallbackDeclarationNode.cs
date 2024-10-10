namespace KSPCompiler.Domain.Ast.Nodes.Blocks
{
    /// <summary>
    /// AST node representing a callback definition
    /// </summary>
    public class AstCallbackDeclarationNode : AstFunctionalNode, IArgumentList
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallbackDeclarationNode() : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallbackDeclarationNode( IAstNode parent )
            : base( AstNodeId.CallbackDeclaration, parent )
        {
            ArgumentList = new AstArgumentListNode( this );
        }

        #region IArgument

        ///
        /// <inheritdoc />
        ///
        public AstArgumentListNode ArgumentList { get; set; }

        #endregion IArgument

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this, abortTraverseToken );


        #endregion IAstNodeAcceptor
    }
}
