using System;

#nullable disable

namespace KSPCompiler.Domain.Ast.Statements
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
        public string Name { get; set; }

        #endregion INamable

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallKspUserFunctionStatement( IAstNode parent = null )
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
