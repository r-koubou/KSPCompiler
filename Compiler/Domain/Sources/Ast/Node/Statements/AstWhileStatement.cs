namespace KSPCompiler.Domain.Ast.Node.Statements
{
    /// <summary>
    /// AST node representing an while statement
    /// </summary>
    public class AstWhileStatement : AstConditionalStatement
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstWhileStatement()
            : this( null )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstWhileStatement( IAstNode? parent = null )
            : base( AstNodeId.WhileStatement, parent )
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
