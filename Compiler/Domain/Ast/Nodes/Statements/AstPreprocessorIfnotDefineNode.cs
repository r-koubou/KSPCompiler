using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// Ast node representing a KSP Preprocessor: USE_CODE_IF_NOT
    /// </summary>
    public class AstPreprocessorIfnotDefineNode : AstStatementNode
    {
        /// <summary>
        /// If true, the block can be ignored. (default: false)
        /// </summary>
        /// <remarks>
        /// Value will be updated in preprocess phase.
        /// </remarks>
        public bool Ignore { get; set; }

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
        public AstPreprocessorIfnotDefineNode()
            : this( NullAstNode.Instance, NullAstExpressionNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorIfnotDefineNode( AstExpressionNode condition )
            : this( NullAstNode.Instance, condition ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorIfnotDefineNode( IAstNode parent, AstExpressionNode condition )
            : base( AstNodeId.PreprocessorIfdefine, parent )
        {
            Condition = condition;
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
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren( IAstVisitor visitor )
        {
            Condition.Accept( visitor );

            foreach( var statement in Block.Statements )
            {
                statement.Accept( visitor );
            }
        }

        #endregion IAstNodeAcceptor
    }
}
