using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a string expression
    /// </summary>
    public class AstStringExpression : AstExpressionSyntaxNode
    {
        public override DataTypeFlag TypeFlag { get; set; } = DataTypeFlag.TypeString;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringExpression( AstNodeId id, IAstNode parent )
            : base( id, parent, NullAstExpressionSyntaxNode.Instance, NullAstExpressionSyntaxNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringExpression( IAstNode parent, AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.StringExpression, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringExpression( AstExpressionSyntaxNode left, AstExpressionSyntaxNode right )
            : base( AstNodeId.StringExpression, NullAstNode.Instance, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstStringExpression()
            : base( AstNodeId.StringExpression, NullAstNode.Instance )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
