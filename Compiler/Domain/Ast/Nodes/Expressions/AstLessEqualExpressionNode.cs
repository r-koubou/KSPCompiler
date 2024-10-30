using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a comparison operator: less equal (&lt;=)
    /// </summary>
    public class AstLessEqualExpressionNode : AstExpressionNode
    {
        public override DataTypeFlag TypeFlag
            => DataTypeFlag.TypeBool;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessEqualExpressionNode( IAstNode parent, AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.LessEqual, parent, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessEqualExpressionNode( AstExpressionNode left, AstExpressionNode right )
            : base( AstNodeId.LessEqual, left, right )
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstLessEqualExpressionNode()
            : base( AstNodeId.LessEqual )
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
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
