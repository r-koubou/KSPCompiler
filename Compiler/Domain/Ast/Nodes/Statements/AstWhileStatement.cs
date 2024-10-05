namespace KSPCompiler.Domain.Ast.Nodes.Statements
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
            : this( NullAstNode.Instance )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstWhileStatement( IAstNode parent )
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
