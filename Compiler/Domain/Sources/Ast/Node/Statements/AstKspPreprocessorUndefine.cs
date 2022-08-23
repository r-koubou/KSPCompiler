using System;

using KSPCompiler.Domain.Ast.Node.Expressions;

namespace KSPCompiler.Domain.Ast.Node.Statements
{
    /// <summary>
    /// AST node representing a KSP Preprocessor: RESET_CONDITION
    /// </summary>
    public class AstKspPreprocessorUndefine : AstStatementSyntaxNode
    {
        /// <summary>
        /// AST node representing a preprocessor symbol.
        /// </summary>
        public AstSymbolExpression Symbol { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorUndefine( IAstNode? parent = null )
            : base( AstNodeId.KspPreprocessorUndefine, parent )
        {
            Symbol = new AstSymbolExpression( this );
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
