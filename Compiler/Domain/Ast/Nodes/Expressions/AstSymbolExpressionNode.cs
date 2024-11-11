using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Nodes.Expressions
{
    /// <summary>
    /// AST node representing a symbolic reference
    /// </summary>
    public class AstSymbolExpressionNode : AstExpressionNode
    {
        /// <summary>
        /// Represents whether the node is symbol(variable, commands, callback, etc.) and its built-in by NI.
        /// </summary>
        public virtual bool BuiltIn { get; set; }

        /// <summary>
        /// Represents a symbol state for evaluation in analysis.
        /// </summary>
        /// <remarks>
        /// Typically, this property will be set in evaluation process as return evaluation result.
        /// </remarks>
        public SymbolState SymbolState { get; set; } = SymbolState.UnInitialized;

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
        public AstSymbolExpressionNode( string name )
            : this( name, NullAstExpressionNode.Instance )
        {
            Name = name;
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
            => 0;

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
