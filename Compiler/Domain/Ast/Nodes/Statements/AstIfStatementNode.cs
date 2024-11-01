using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
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
        {
            get
            {
                var result = 1; // condition node is always present

                if( CodeBlock != NullAstNode.Instance )
                {
                    result++;
                }

                if( ElseBlock != NullAstNode.Instance )
                {
                    result++;
                }

                return result;
            }
        }

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        #endregion IAstNodeAcceptor
    }
}
