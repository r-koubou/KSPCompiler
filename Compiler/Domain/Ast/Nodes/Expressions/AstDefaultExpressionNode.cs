using System;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// Basic implementation of <see cref="AstExpressionNode"/>
    /// </summary>
    /// <remarks>
    /// There are no contracts for node IDs or data types.
    /// If there are concrete expressions, symbols, or literals, they should be inherited to create concrete classes.
    /// The case for directly creating an instance of this class assumes the analysis process.
    /// </remarks>
    public class AstDefaultExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Null Object of <see cref="AstDefaultExpressionNode"/>
        /// </summary>
        /// <remarks>
        /// If invalid by analyzing the code the result set to this.
        /// </remarks>
        public static readonly AstDefaultExpressionNode Null = new ();

        /// <summary>
        /// Represents whether the node is symbol(variable, commands, callback, etc.) and its reserved (built-in) by NI.
        /// </summary>
        [Obsolete("Should be gotten from the symbol table. This property will be removed in the future.")]
        public virtual bool Reserved { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        private AstDefaultExpressionNode()
            : this( AstNodeId.None, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDefaultExpressionNode( AstNodeId id )
            : base( id, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDefaultExpressionNode( IAstNode parent )
            : base( AstNodeId.None, parent ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstDefaultExpressionNode( AstNodeId id, IAstNode parent )
            : base( id, parent ) {}

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public override int ChildNodeCount
            => 2;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
