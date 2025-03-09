namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks
{
    /// <summary>
    /// Ast node representing a case block in the select statement
    /// </summary>
    public class AstCaseBlock : AstNode
    {
        /// <summary>
        /// Conditional expression (starting value)
        /// </summary>
        public AstExpressionNode ConditionFrom { get; set; }

        /// <summary>
        /// Conditional expression (end value *optional)
        /// </summary>
        public AstExpressionNode ConditionTo { get; set; }

        /// <summary>
        /// Code block to be executed when the case condition is matched
        /// </summary>
        public  AstBlockNode CodeBlock { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCaseBlock()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCaseBlock( IAstNode parent )
            : this( parent, NullAstExpressionNode.Instance, NullAstExpressionNode.Instance, new AstBlockNode() ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCaseBlock( IAstNode parent, AstExpressionNode conditionFrom, AstExpressionNode conditionTo, AstBlockNode codeBlock )
            : base( AstNodeId.CaseBlock, parent )
        {
            ConditionFrom = conditionFrom;
            ConditionTo   = conditionTo;
            CodeBlock     = codeBlock;
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 3;

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        public override void AcceptChildren( IAstVisitor visitor )
        {
            ConditionFrom.Accept( visitor );
            ConditionTo.Accept( visitor );
            CodeBlock.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
