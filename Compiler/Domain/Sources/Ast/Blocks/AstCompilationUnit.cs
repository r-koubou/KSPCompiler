namespace KSPCompiler.Domain.Ast.Blocks
{
    /// <summary>
    /// AST node representing a root node
    /// </summary>
    public class AstCompilationUnit : AstNode
    {
        /// <summary>
        /// Global blocks for callback definitions, user-defined functions, etc.
        /// </summary>
        public AstNodeList<AstNode> GlobalBlocks { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCompilationUnit( IAstNode? parent = null )
            : base( AstNodeId.CompilationUnit, parent )
        {
            GlobalBlocks = new AstNodeList<AstNode>( this );
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
            foreach( var n in GlobalBlocks )
            {
                n.Accept( visitor );
            }
        }

        #endregion IAstNodeAcceptor
    }
}
