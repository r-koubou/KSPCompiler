namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a string concatenation operator (&)
    /// </summary>
    public class AstStringConcatenateExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringConcatenateExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.StringConcatenate, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringConcatenateExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.StringConcatenate,  left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringConcatenateExpressionNode()
            : base( AstNodeId.StringConcatenate )
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
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
