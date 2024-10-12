using KSPCompiler.Domain.Ast.Nodes.Extensions;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a symbolic reference
    /// </summary>
    public class AstSymbolExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstSymbolExpressionNode( string name, IAstNode parent, AstExpressionNode left )
            : base( AstNodeId.Symbol, parent, left, NullAstExpressionNode.Instance )
        {
            Name = name;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSymbolExpressionNode( string name, AstExpressionNode left )
            : base( AstNodeId.Symbol, left, NullAstExpressionNode.Instance )
        {
            Name = name;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSymbolExpressionNode( IAstNode parent, AstExpressionNode left )
            : this( string.Empty, parent, left )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSymbolExpressionNode( AstExpressionNode left )
            : this( string.Empty, left )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSymbolExpressionNode()
            : base( AstNodeId.Symbol )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => Left.IsNotNull() ? 1 : 0;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
