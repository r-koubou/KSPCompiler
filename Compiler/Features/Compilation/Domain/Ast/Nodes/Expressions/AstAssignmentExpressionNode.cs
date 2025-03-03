namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an assignment (:=)
    /// </summary>
    public class AstAssignmentExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Assign type definition
        /// </summary>
        public enum OperatorType
        {
            /// <summary>
            /// :=
            /// </summary>
            Assign
        }

        /// <summary>
        ///  Operator type for assigning
        /// </summary>
        public OperatorType Operator { get; set; } = OperatorType.Assign;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignmentExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.AssignmentExpression, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignmentExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.AssignmentExpression, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignmentExpressionNode()
            : base( AstNodeId.AssignmentExpression )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 2;
            // Left: Destination
            // Right: Source

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
