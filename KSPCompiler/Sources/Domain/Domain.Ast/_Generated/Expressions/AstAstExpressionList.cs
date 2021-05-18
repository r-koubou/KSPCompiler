// Generated by CodeGenerators/ASTCodeGenerator

namespace KSPCompiler.Domain.Ast.Expressions
{
    /// <summary>
    /// AST node representing a comma-separated list of expressions.
    /// </summary>
    public partial class AstAstExpressionList : AstExpressionSyntaxNode
    {
        #region Fields
        /// <summary>
        /// Expression list
        /// </summary>
        public AstNodeList<AstExpressionSyntaxNode> Expressions { get; }

        #endregion Fields

        /// <summary>
        /// Ctor.
        /// </summary>
        public AstAstExpressionList() : base( AstNodeId.AstExpressionList, IAstNode.None )
        {
            Expressions = new AstNodeList<AstExpressionSyntaxNode>( this );
        }
        
        public AstAstExpressionList( IAstNode parent ) : base( AstNodeId.AstExpressionList, parent )
        {
            Expressions = new AstNodeList<AstExpressionSyntaxNode>( this );
        }


        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            foreach( var n in Expressions )
            {
                n.Accept( visitor );
            }
        }

        #endregion IAstNodeAcceptor
    }
}
