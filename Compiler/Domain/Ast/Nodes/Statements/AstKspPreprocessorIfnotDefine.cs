using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// Ast node representing a KSP Preprocessor: USE_CODE_IF_NOT
    /// </summary>
    public class AstKspPreprocessorIfnotDefine : AstStatementSyntaxNode
    {
        /// <summary>
        /// The ifndef conditional symbol.
        /// </summary>
        public AstDefaultExpression Condition { get; }

        /// <summary>
        /// The code block for ifndef is true.
        /// </summary>
        public AstBlock Block { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorIfnotDefine()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorIfnotDefine( IAstNode parent )
            : base( AstNodeId.KspPreprocessorIfnotDefine, parent )
        {
            Condition = new AstDefaultExpression( this );
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
