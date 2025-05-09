namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks
{
    /// <summary>
    /// AST node representing a callback, block in function
    /// </summary>
    public class AstBlockNode : AstNode
    {
        /// <summary>
        /// Statements
        /// </summary>
        public AstNodeList<AstNode> Statements { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBlockNode() : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstBlockNode( IAstNode parent )
            : base( AstNodeId.Block, parent )
        {
            Statements = new AstNodeList<AstNode>( this );
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => Statements.Count;

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
            foreach( var n in Statements )
            {
                n.Accept( visitor );
            }
        }

        #endregion IAstNodeAcceptor
    }
}
