#nullable disable

using System.IO;

namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// The base node representing the expression.
    /// </summary>
    public abstract class AstExpressionSyntaxNode : AstNode
    {
        /// <summary>
        /// left operand
        /// </summary>
        public AstExpressionSyntaxNode Left { get; set; }

        /// <summary>
        /// right operand
        /// </summary>
        public AstExpressionSyntaxNode Right { get; set; }

        /// <summary>
        /// The data type representing this node
        /// </summary>
        public DataType Type { get; set; } = DataType.Void;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionSyntaxNode()
            : base( AstNodeId.None, IAstNode.None )
        {
            Left  = null;
            Right = null;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionSyntaxNode(
            AstNodeId id,
            IAstNode parent )
            : this( id, parent, null, null )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionSyntaxNode(
            AstNodeId id,
            IAstNode parent,
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : base( id, parent )
        {
            Left  = left;
            Right = right;
        }

        /// <summary>
        /// Returns whether the node is a constant expressive node or not.
        /// </summary>
        public virtual bool IsConstant => false;

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            Left.AcceptChildren( visitor );
            Right.AcceptChildren( visitor );
        }
        #endregion IAstNodeAcceptor

        #region AstNode
        ///
        /// <inheritdoc/>
        ///
        public override void DumpAll( StreamWriter writer, int indentDepth = 0 )
        {
            Left.Dump( writer, indentDepth );
            Right.Dump( writer, indentDepth );
        }
        #endregion AstNode
    }
}
