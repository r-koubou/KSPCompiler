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
        public AstExpressionSyntaxNode ConditionFrom { get; set; }

        /// <summary>
        /// Conditional expression (end value *optional)
        /// </summary>
        public AstExpressionSyntaxNode ConditionTo { get; set; }

        /// <summary>
        /// Code block to be executed when the case condition is matched
        /// </summary>
        public  AstBlock CodeBlock { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCaseBlock()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCaseBlock( IAstNode parent )
            : this( parent, NullAstExpressionSyntaxNode.Instance, NullAstExpressionSyntaxNode.Instance, new AstBlock() ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCaseBlock( IAstNode parent, AstExpressionSyntaxNode conditionFrom, AstExpressionSyntaxNode conditionTo, AstBlock codeBlock )
            : base( AstNodeId.CaseBlock, parent )
        {
            ConditionFrom = conditionFrom;
            ConditionTo   = conditionTo;
            CodeBlock     = codeBlock;
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
