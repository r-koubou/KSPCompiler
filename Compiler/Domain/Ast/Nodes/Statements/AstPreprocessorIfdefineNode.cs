using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a KSP Preprocessor: USE_CODE_IF
    /// </summary>
    public class AstPreprocessorIfdefineNode : AstStatementNode
    {
        /// <summary>
        /// If true, the block can be ignored. (default: false)
        /// </summary>
        /// <remarks>
        /// Value will be updated in preprocess phase.
        /// </remarks>
        public bool Ignore { get; set; }

        /// <summary>
        /// The ifdef conditional symbol.
        /// </summary>
        public AstExpressionNode Condition { get; set; }

        /// <summary>
        /// The code block for ifdef is true.
        /// </summary>
        public AstBlockNode Block { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorIfdefineNode()
            : this( NullAstNode.Instance, NullAstExpressionNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorIfdefineNode( AstExpressionNode condition )
            : this( NullAstNode.Instance, condition ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstPreprocessorIfdefineNode( IAstNode parent, AstExpressionNode condition )
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
            Block.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
