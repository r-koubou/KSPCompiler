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
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 2; // condition & code block

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
