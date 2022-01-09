namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing an assignment (:=)
    /// </summary>
    public class AstAssignmentExpression : AstExpressionSyntaxNode
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
        public AstAssignmentExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.AssignmentExpression, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignmentExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.AssignmentExpression, null, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignmentExpression()
            : base( AstNodeId.AssignmentExpression, null )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
