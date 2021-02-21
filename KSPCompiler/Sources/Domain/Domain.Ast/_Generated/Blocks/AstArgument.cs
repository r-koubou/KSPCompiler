// Generated by CodeGenerators/ASTCodeGenerator/ast_gen.py


namespace KSPCompiler.Domain.Ast.Blocks
{
    /// <summary>
    /// AST node representing an argument
    /// </summary>
    public partial class AstArgument : AstNode, INameable
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstArgument( IAstNode parent )
            : base( AstNodeId.Argument, parent )
        {
            Initialize();
        }

        private partial void Initialize();

        #region Part of IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
        {
            return visitor.Visit( this );
        }
        #endregion IAstNodeAcceptor
    }
}