#nullable disable

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing a symbolic reference
    /// </summary>
    public class AstSymbolExpression : AstExpressionSyntaxNode, INameable
    {
        #region INameable

        ///
        /// <inheritdoc/>
        ///
        public string Name { get; set; } = "";

        #endregion INameable

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSymbolExpression( IAstNode parent = null )
            : base( AstNodeId.Symbol, parent, null, null )
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
