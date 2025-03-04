using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a call statement
    /// </summary>
    public class AstCallUserFunctionStatementNode : AstStatementNode
    {
        /// <summary>
        /// An user function name
        /// </summary>
        public AstSymbolExpressionNode Symbol { get; set; } = new();

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallUserFunctionStatementNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallUserFunctionStatementNode( IAstNode parent )
            : base( AstNodeId.CallUserFunctionStatement, parent )
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

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren( IAstVisitor visitor )
        {
            // Do nothing
        }

        #endregion IAstNodeAcceptor
    }
}
