using System;

using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a KSP Preprocessor: RESET_CONDITION
    /// </summary>
    public class AstKspPreprocessorUndefineNode : AstStatementNode
    {
        /// <summary>
        /// AST node representing a preprocessor symbol.
        /// </summary>
        public AstExpressionNode Symbol { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorUndefineNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorUndefineNode( IAstNode parent )
            : base( AstNodeId.KspPreprocessorUndefine, parent )
        {
            Symbol = new AstDefaultExpressionNode( this );
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
