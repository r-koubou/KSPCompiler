using System;

using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Expressions;

namespace KSPCompiler.Domain.Ast.Node.Statements
{
    /// <summary>
    /// AST node representing a KSP Preprocessor: USE_CODE_IF
    /// </summary>
    public class AstKspPreprocessorIfdefine : AstStatementSyntaxNode
    {
        /// <summary>
        /// The ifdef conditional symbol.
        /// </summary>
        public AstSymbolExpression Condition { get; }

        /// <summary>
        /// The code block for ifdef is true.
        /// </summary>
        public AstBlock? Block { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorIfdefine( IAstNode? parent = null )
            : base( AstNodeId.KspPreprocessorIfdefine, parent )
        {
            Condition = new AstSymbolExpression( this );
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor )
        {
            throw new NotImplementedException();
        }

        #endregion IAstNodeAcceptor
    }
}
