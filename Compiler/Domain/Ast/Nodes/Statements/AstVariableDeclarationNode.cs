namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a variable declaration
    /// </summary>
    public class AstVariableDeclarationNode : AstStatementNode, INameable
    {
        /// <summary>
        /// modifier
        /// </summary>
        public string Modifier { get; set; } = string.Empty;

        #region INamable

        /// <summary>
        /// variable name
        /// </summary>
        public string Name { get; set; } = "";

        #endregion

        /// <summary>
        /// initialization statement
        /// </summary>
        public AstInitializerNode Initializer { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableDeclarationNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableDeclarationNode( IAstNode parent )
            : base( AstNodeId.VariableDeclaration, parent )
        {
            Initializer = NullAstInitializerNode.Instance;
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 1;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            Initializer.AcceptChildren( visitor, abortTraverseToken );
        }

        #endregion IAstNodeAcceptor
    }
}
