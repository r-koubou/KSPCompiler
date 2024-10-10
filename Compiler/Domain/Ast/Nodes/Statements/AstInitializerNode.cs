namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing an initialization statement
    /// </summary>
    public abstract class AstInitializerNode : AstStatementNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected AstInitializerNode( AstNodeId id, IAstNode parent  )
            : base( id, parent ) {}
    }
}
