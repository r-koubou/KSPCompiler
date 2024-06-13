namespace KSPCompiler.Domain.Ast.Node.Blocks
{
    /// <summary>
    /// Ast node representing a case block in the select statement
    /// </summary>
    public class AstCaseBlock : AstNode
    {
        /// <summary>
        /// Conditional expression (starting value)
        /// </summary>
        public AstExpressionSyntaxNode ConditionFrom { get; set; } = NullAstExpressionSyntaxNode.Instance;

        /// <summary>
        /// Conditional expression (end value *optional)
        /// </summary>
        public AstExpressionSyntaxNode ConditionTo { get; set; } = NullAstExpressionSyntaxNode.Instance;

        /// <summary>
        /// Code block to be executed when the case condition is matched
        /// </summary>
        public  AstBlock CodeBlock { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCaseBlock()
            : this( NullAstNode.Instance )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCaseBlock( IAstNode parent )
            : base( AstNodeId.CaseBlock, parent )
        {
            CodeBlock = new AstBlock( this );
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            CodeBlock.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
