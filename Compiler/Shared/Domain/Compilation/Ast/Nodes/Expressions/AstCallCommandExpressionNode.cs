namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an invoking the KSP command
    /// </summary>
    /// <remarks>
    /// Left: <see cref="AstSymbolExpressionNode"/> (Command name)<br/>
    /// Right: <see cref="AstExpressionListNode"/> (Arguments)
    /// </remarks>
    public class AstCallCommandExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallCommandExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.CallCommandExpression, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallCommandExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.CallCommandExpression, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallCommandExpressionNode()
            : base( AstNodeId.CallCommandExpression )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 2;
            // Left: Command name
            // Right: Arguments

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
