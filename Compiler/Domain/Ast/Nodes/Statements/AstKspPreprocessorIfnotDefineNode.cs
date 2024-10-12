using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// Ast node representing a KSP Preprocessor: USE_CODE_IF_NOT
    /// </summary>
    public class AstKspPreprocessorIfnotDefineNode : AstStatementNode
    {
        /// <summary>
        /// The ifndef conditional symbol.
        /// </summary>
        public AstExpressionNode Condition { get; }

        /// <summary>
        /// The code block for ifndef is true.
        /// </summary>
        public AstBlockNode Block { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorIfnotDefineNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorIfnotDefineNode( IAstNode parent )
            : base( AstNodeId.KspPreprocessorIfnotDefine, parent )
        {
            Condition = new AstDefaultExpressionNode( this );
            Block     = new AstBlockNode( this );
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
