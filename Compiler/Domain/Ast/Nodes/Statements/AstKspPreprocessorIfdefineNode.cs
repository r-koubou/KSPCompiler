using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a KSP Preprocessor: USE_CODE_IF
    /// </summary>
    public class AstKspPreprocessorIfdefineNode : AstStatementNode
    {
        /// <summary>
        /// The ifdef conditional symbol.
        /// </summary>
        public AstDefaultExpressionNode Condition { get; }

        /// <summary>
        /// The code block for ifdef is true.
        /// </summary>
        public AstBlockNode Block { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorIfdefineNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstKspPreprocessorIfdefineNode( IAstNode parent )
            : base( AstNodeId.KspPreprocessorIfdefine, parent )
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
