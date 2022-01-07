#nullable disable

using KSPCompiler.Domain.Ast.Blocks;

namespace KSPCompiler.Domain.Ast.Statements
{
    /// <summary>
    /// AST node representing an if statement
    /// </summary>
    public class AstIfStatement : AstConditionalStatement
    {
        /// <summary>
        /// The code block when else is satisfied.
        /// </summary>
        public AstBlock ElseBlock { get; set; }

        /// <summary>s
        /// Ctor
        /// </summary>
        public AstIfStatement()
            : this( null )
        {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIfStatement( IAstNode parent = null )
            : base( AstNodeId.IfStatement, parent )
        {
            ElseBlock = new AstBlock( this );
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
