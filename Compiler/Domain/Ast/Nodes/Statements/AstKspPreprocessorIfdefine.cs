using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
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
        public AstBlock Block { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorIfdefine()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorIfdefine( IAstNode parent )
            : base( AstNodeId.KspPreprocessorIfdefine, parent )
        {
            Condition = new AstSymbolExpression( this );
            Block     = new AstBlock( this );
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 2;

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
