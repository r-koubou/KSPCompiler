namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// AST node representing a base node representing of a statement.
    /// </summary>
    public abstract class AstStatementSyntaxNode : AstNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected AstStatementSyntaxNode(
            AstNodeId id,
            IAstNode? parent )
            : base( id, parent )
        {}
    }
}
