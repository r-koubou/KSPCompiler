using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a select statement
    /// </summary>
    public class AstSelectStatementNode : AstStatementNode
    {
        /// <summary>
        /// condition
        /// </summary>
        public AstExpressionNode Condition { get; set; }

        /// <summary>
        /// case list
        /// </summary>
        public AstNodeList<AstCaseBlock> CaseBlocks { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSelectStatementNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSelectStatementNode( IAstNode parent )
            : base( AstNodeId.SelectStatement, parent )
        {
            Condition  = NullAstExpressionNode.Instance;
            CaseBlocks = new AstNodeList<AstCaseBlock>( this );
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 2;

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
            Condition.Accept( visitor );

            foreach( var n in CaseBlocks )
            {
                n.Accept( visitor );
            }
        }

        #endregion IAstNodeAcceptor
    }
}
