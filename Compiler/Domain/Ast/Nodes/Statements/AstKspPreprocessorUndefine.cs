using System;

using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
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
        public AstKspPreprocessorUndefine()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorUndefine( IAstNode parent )
            : base( AstNodeId.KspPreprocessorUndefine, parent )
        {
            Symbol = new AstSymbolExpression( this );
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 1;

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
        {
            throw new NotImplementedException();
        }

        #endregion IAstNodeAcceptor
    }
}
