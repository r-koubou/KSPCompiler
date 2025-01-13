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
        public AstSymbolExpressionNode( IAstNode parent, string name )
            : base( AstNodeId.Symbol, parent, NullAstExpressionNode.Instance, NullAstExpressionNode.Instance )
        {
            Name = name;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstSymbolExpressionNode( string name )
            : this()
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
