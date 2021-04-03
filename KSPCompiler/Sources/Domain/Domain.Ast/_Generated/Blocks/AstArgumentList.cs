// Generated by CodeGenerators/ASTCodeGenerator

namespace KSPCompiler.Domain.Ast.Blocks
{
    /// <summary>
    /// AST node representing an argument list
    /// </summary>
    public partial class AstArgumentList : AstNode
    {
        #region Fields
        /// <summary>
        /// public AstNodeList<AstArgument> Arguments { get; } = new();
        /// </summary>
        public AstNodeList<AstArgument> Arguments { get; } = new();

        public bool HasArgument => Arguments.Count > 0;

        public int ArgumentCount => Arguments.Count;

        #endregion Fields

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgumentList( IAstNode parent )
            : base( AstNodeId.ArgumentList, parent )
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
        {}

        #endregion IAstNodeAcceptor
    }
}
