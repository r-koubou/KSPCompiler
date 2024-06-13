namespace KSPCompiler.Domain.Ast.Node.Statements
{
    /// <summary>
    /// AST node representing an initialization statement
    /// </summary>
    public abstract class AstInitializer : AstStatementSyntaxNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected AstInitializer( AstNodeId id, IAstNode parent  )
            : base( id, parent ) {}
    }
}