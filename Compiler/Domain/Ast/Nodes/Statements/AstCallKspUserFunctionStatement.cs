using System;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a call statement
    /// </summary>
    public class AstCallKspUserFunctionStatement : AstStatementSyntaxNode, INameable
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
        public AstCallKspUserFunctionStatement()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallKspUserFunctionStatement( IAstNode parent )
            : base( AstNodeId.CallKspUserFunctionStatement, parent )
        {
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