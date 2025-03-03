using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing an if statement
    /// </summary>
    public class AstIfStatementNode : AstConditionalStatementNode
    {
        /// <summary>
        /// The code block when else is satisfied.
        /// </summary>
        public AstBlockNode ElseBlock { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIfStatementNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstIfStatementNode( IAstNode parent )
            : base( AstNodeId.IfStatement, parent )
        {
            ElseBlock = new AstBlockNode( this );
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 3;
            // Condition
            // CodeBlock
            // ElseBlock

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        /// <summary>
        /// Base class AcceptChildren + ElseBlock
        /// </summary>
        /// <param name="visitor">Visitor for traversing the AST</param>
        public override void AcceptChildren( IAstVisitor visitor )
        {
            base.AcceptChildren( visitor );
            ElseBlock.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
