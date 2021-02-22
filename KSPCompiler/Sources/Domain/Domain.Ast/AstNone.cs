using KSPCompiler.Domain.TextFile.Aggregate;

namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// The Empty Ast Object
    /// </summary>
    public partial class AstNone : AstNode
    {
        public AstNone() : base( AstNodeId.None, IAstNode.None )
        {}

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
        {
            return visitor.Visit( this );
        }

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            // Do nothing
        }
        #endregion IAstNodeAcceptor
    }
}