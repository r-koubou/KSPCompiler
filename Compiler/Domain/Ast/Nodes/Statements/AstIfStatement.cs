using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
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

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIfStatement()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIfStatement( IAstNode parent )
            : base( AstNodeId.IfStatement, parent )
        {
            ElseBlock = new AstBlock( this );
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override T Accept<T>( IAstVisitor<T> visitor, AbortTraverseToken abortTraverseToken )
            => visitor.Visit( this , abortTraverseToken );

        #endregion IAstNodeAcceptor
    }
}
