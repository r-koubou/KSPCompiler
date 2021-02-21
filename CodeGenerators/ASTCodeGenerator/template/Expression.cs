namespace __namespace__
{
    /// <summary>
    /// Ast node representing __desc__
    /// </summary>
    public partial class __name__ : __base__
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public __name__(
            IAstNode parent,
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base(
                AstNodeId.__ast_id__,
                parent,
                left, right )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public __name__(
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base(
                AstNodeId.__ast_id__,
                IAstNode.None,
                left, right )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public __name__()
            : base(
                AstNodeId.__ast_id__,
                IAstNode.None,
                AstExpressionSyntaxNode.None,
                AstExpressionSyntaxNode.None )
        {}
    }
}