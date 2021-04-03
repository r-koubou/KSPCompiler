// Generated by CodeGenerators/ASTCodeGenerator
using KSPCompiler.Domain.Ast.Expressions;

namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing a KSP Preprocessor - USE_CODE_IF
    /// </summary>
    public partial class AstKspPreprocessorIfdefine : AstStatementSyntaxNode
    {
        #region Fields
        /// <summary>
        /// public AstSymbolExpression Condition { get; }
        /// </summary>
        public AstSymbolExpression Condition { get; }

        /// <summary>
        /// public AstBlock? Block { get; set; }
        /// </summary>
        public AstBlock? Block { get; set; }

        #endregion Fields

        /// <summary>
        /// Ctor.
        /// </summary>
        public AstKspPreprocessorIfdefine( IAstNode parent )
            : base( AstNodeId.KspPreprocessorIfdefine, parent )
        {
            Condition = new AstSymbolExpression( this );
        }


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
