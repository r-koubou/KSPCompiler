using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing an integer expression
    /// </summary>
    public abstract class AstIntExpression : AstExpressionSyntaxNode
    {
        public override DataTypeFlag TypeFlag { get; set; } = DataTypeFlag.TypeInt;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntExpression( AstNodeId id, IAstNode parent )
            : base( id, parent, NullAstExpressionSyntaxNode.Instance, NullAstExpressionSyntaxNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.IntExpression, parent, left, right ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.IntExpression, NullAstNode.Instance, left, right ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIntExpression()
            : base( AstNodeId.IntExpression, NullAstNode.Instance ) {}

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
