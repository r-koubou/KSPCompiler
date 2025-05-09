namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks
{
    /// <summary>
    /// AST node representing a root node
    /// </summary>
    public class AstCompilationUnitNode : AstNode
    {
        /// <summary>
        /// Global blocks for callback definitions, user-defined functions, etc.
        /// </summary>
        public AstNodeList<AstNode> GlobalBlocks { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCompilationUnitNode()
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
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren( IAstVisitor visitor )
        {
            foreach( var n in GlobalBlocks )
            {
                n.Accept( visitor );
            }
        }

        #endregion IAstNodeAcceptor
    }
}
