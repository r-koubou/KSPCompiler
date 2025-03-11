namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes
{
    /// <summary>
    /// AST node representing a base node representing of a statement.
    /// </summary>
    public abstract class AstStatementNode : AstNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected AstStatementNode(
            AstNodeId id,
            IAstNode parent )
            : base( id, parent )
        {}
    }
}
