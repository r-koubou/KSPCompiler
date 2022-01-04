namespace ${namespace}
{
    /// <summary>
    /// AST node representing ${description}
    /// </summary>
    public class ${classname} : AstStatementSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public ${classname}( IAstNode parent = null )
            : base( AstNodeId.${name}, parent )
        {
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( KSPCompiler.Domain.Ast.IAstVisitor<T> visitor )
        {
            throw new System.NotImplementedException();
        }
        #endregion IAstNodeAcceptor
    }
}
