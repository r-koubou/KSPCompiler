using KSPCompiler.Domain.Ast.Blocks;
using KSPCompiler.Domain.Ast.Expressions;

namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing a base node of a conditional branching statement.
    /// </summary>
    public abstract class AstConditionalStatement : AstStatementSyntaxNode
    {
        /// <summary>
        /// Condition
        /// </summary>
        public AstExpressionSyntaxNode Condition { get; set; }

        /// <summary>
        /// A code block when the conditional expression is true.
        /// </summary>
        public AstBlock CodeBlock { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        protected AstConditionalStatement(
            AstNodeId id,
            IAstNode parent )
            : base( id, parent )
        {
        }

        #region IAstNodeAcceptor
        /// <summary>
        /// Do nothing. Override as appropriate.
        /// </summary>
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {}
        #endregion
    }
}
