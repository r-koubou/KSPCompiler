namespace KSPCompiler.Domain.Ast.Nodes.Blocks
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
        public AstCompilationUnit()
            : base( AstNodeId.CompilationUnit, NullAstNode.Instance )
        {
            GlobalBlocks = new AstNodeList<AstNode>( this );
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => GlobalBlocks.Count;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            if( abortTraverseToken.Aborted )
            {
                return;
            }

            foreach( var n in GlobalBlocks )
            {
                n.Accept( visitor, abortTraverseToken );

                if( abortTraverseToken.Aborted )
                {
                    return;
                }
            }
        }

        #endregion IAstNodeAcceptor
    }
}
