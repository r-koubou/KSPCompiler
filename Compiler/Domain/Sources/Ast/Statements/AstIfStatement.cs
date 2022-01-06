#nullable disable

namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing an if statement
    /// </summary>
    public class AstIfStatement : AstStatementSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstIfStatement( IAstNode parent = null )
            : base( AstNodeId.IfStatement, parent )
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
            throw new System.NotImplementedException();
        }

        #endregion IAstNodeAcceptor
    }
}
