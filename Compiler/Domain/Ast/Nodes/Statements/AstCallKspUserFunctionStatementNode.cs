using System;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a call statement
    /// </summary>
    public class AstCallKspUserFunctionStatementNode : AstStatementNode, INameable
    {
        #region INamable

        /// <summary>
        /// An user function name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        #endregion INamable

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallKspUserFunctionStatementNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallKspUserFunctionStatementNode( IAstNode parent )
            : base( AstNodeId.CallKspUserFunctionStatement, parent )
        {
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 0;

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
