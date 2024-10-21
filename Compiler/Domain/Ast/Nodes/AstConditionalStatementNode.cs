using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes
{
    /// <summary>
    /// AST node representing a base node of a conditional branching statement.
    /// </summary>
    public abstract class AstConditionalStatementNode : AstStatementNode
    {
        /// <summary>
        /// Condition
        /// </summary>
        public AstExpressionNode Condition { get; set; }

        /// <summary>
        /// A code block when the conditional expression is true.
        /// </summary>
        public AstBlockNode CodeBlock { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        protected AstConditionalStatementNode(
            AstNodeId id,
            IAstNode parent )
            : base( id, parent )
        {
            Condition = NullAstExpressionNode.Instance;
            CodeBlock = new AstBlockNode( this );
        }

        #region IAstNodeAcceptor

        /// <summary>
        /// Call Condition, CodeBlock's <see cref="AcceptChildren{T}"/>
        /// </summary>
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            Condition.AcceptChildren( visitor );
            CodeBlock.AcceptChildren( visitor );
        }

        #endregion
    }
}
