#nullable disable

namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing a variable initialization
    /// </summary>
    public class AstVariableInitializer : AstNode
    {
        /// <summary>
        /// primitive variable initialization
        /// </summary>
        public AstPrimitiveInitializer PrimitiveInitializer { get; set; }

        /// <summary>
        /// array variable initialization
        /// </summary>
        public AstArrayInitializer ArrayInitializer { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableInitializer( IAstNode parent = null )
            : base( AstNodeId.VariableInitializer, parent )
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
            PrimitiveInitializer?.AcceptChildren( visitor );
            ArrayInitializer?.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
