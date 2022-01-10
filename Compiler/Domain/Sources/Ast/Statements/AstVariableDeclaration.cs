namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing a variable declaration
    /// </summary>
    public class AstVariableDeclaration : AstStatementSyntaxNode, INameable
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
        public AstVariableInitializer? Initializer { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableDeclaration( IAstNode? parent = null )
            : base( AstNodeId.VariableDeclaration, parent )
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
            Initializer?.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
