namespace KSPCompiler.Domain.Ast.Nodes.Expressions
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
            => 1;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
