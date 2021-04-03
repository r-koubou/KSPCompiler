// Generated by CodeGenerators/ASTCodeGenerator
using KSPCompiler.Domain.Ast.Expressions;

namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing an array variable initialization
    /// </summary>
    public partial class AstArrayInitializer : AstStatementSyntaxNode
    {
        #region Fields
        /// <summary>
        /// public AstExpressionSyntaxNode? Size { get; set; }
        /// </summary>
        public AstExpressionSyntaxNode? Size { get; set; }

        /// <summary>
        /// public AstExpressionList? Initializer { get; set; }
        /// </summary>
        public AstExpressionList? Initializer { get; set; }

        #endregion Fields

        /// <summary>
        /// Ctor
        /// </summary>
        public AstArrayInitializer( IAstNode parent )
            : base( AstNodeId.ArrayInitializer, parent )
        {}

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            throw new System.NotImplementedException();
        }

        #endregion IAstNodeAcceptor
    }
}
