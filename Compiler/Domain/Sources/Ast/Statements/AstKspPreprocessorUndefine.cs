#nullable disable

namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing a KSP Preprocessor: RESET_CONDITION
    /// </summary>
    public class AstKspPreprocessorUndefine : AstStatementSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorUndefine( IAstNode parent = null )
            : base( AstNodeId.KspPreprocessorUndefine, parent )
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
