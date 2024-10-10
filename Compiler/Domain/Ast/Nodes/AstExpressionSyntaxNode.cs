using System.IO;

using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Ast.Nodes
{
    /// <summary>
    /// The base node representing the expression.
    /// </summary>
    public abstract class AstExpressionSyntaxNode : AstNode, INameable
    {
        #region INameable

        ///
        /// <inheritdoc/>
        ///
        public string Name { get; set; }  = string.Empty;

        #endregion INameable

        private AstExpressionSyntaxNode left;

        /// <summary>
        /// left operand
        /// </summary>
        public virtual AstExpressionSyntaxNode Left
        {
            get => left;
            set => left = value;
        }

        private AstExpressionSyntaxNode right;

        /// <summary>
        /// right operand
        /// </summary>
        public virtual AstExpressionSyntaxNode Right
        {
            get => right;
            set => right = value;
        }

        /// <summary>
        /// The data type representing this node
        /// </summary>
        public virtual DataTypeFlag TypeFlag { get; set; } = DataTypeFlag.TypeVoid;

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionSyntaxNode()
            : this( AstNodeId.None, NullAstNode.Instance, NullAstExpressionSyntaxNode.Instance, NullAstExpressionSyntaxNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionSyntaxNode( AstNodeId id )
            : this( id, NullAstNode.Instance, NullAstExpressionSyntaxNode.Instance, NullAstExpressionSyntaxNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionSyntaxNode( AstNodeId id, IAstNode parent )
            : this( id, parent, NullAstExpressionSyntaxNode.Instance, NullAstExpressionSyntaxNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstExpressionSyntaxNode(
            AstNodeId id,
            AstExpressionSyntaxNode left,
            AstExpressionSyntaxNode right )
            : this( id, NullAstExpressionSyntaxNode.Instance, left, right ) {}

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
            this.left  = left;
            this.right = right;
        }

        /// <summary>
        /// Returns whether the node is a constant expressive node or not.
        /// </summary>
        public virtual bool IsConstant => false;

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        /// <remarks>
        /// However, if it has more than two child nodes, such as unary operators, literals, or other subclasses, it must override this and return the appropriate value.</remarks>
        ///
        // public override int ChildNodeCount
        //     => 2;

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            if( abortTraverseToken.Aborted )
            {
                return;
            }

            Left.AcceptChildren( visitor, abortTraverseToken );
            Right.AcceptChildren( visitor, abortTraverseToken );
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
