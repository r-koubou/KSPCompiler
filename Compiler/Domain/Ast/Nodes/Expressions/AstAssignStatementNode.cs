namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an assignment
    /// </summary>
    public class AstAssignStatementNode : AstStatementNode
    {
        /// <summary>
        /// Left side of the assignment
        /// </summary>
        public AstExpressionNode Left { get; set; }

        /// <summary>
        /// Right side of the assignment
        /// </summary>
        public AstExpressionNode Right { get; set; }

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
        public AstAssignStatementNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.AssignStatement, parent )
        {
            Left  = left;
            Right = right;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignStatementNode( AstExpressionNode left, AstExpressionNode right )
            : this( NullAstNode.Instance, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstAssignStatementNode()
            : this( NullAstNode.Instance, NullAstExpressionNode.Instance, NullAstExpressionNode.Instance )
        {
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
            Left.Accept( visitor );
            Right.Accept( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
