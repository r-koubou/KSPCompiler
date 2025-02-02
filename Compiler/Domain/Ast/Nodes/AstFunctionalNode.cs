using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes
{
    /// <summary>
    /// The base class of the node corresponding to the function or callback definition.
    /// </summary>
    public abstract class AstFunctionalNode : AstNode, INameable
    {
        /// <summary>
        /// Position of prefix function or callback keyword ("on", "function", etc.)
        /// </summary>
        public Position BeginOnKeywordPosition { get; set; } = new();

        /// <summary>
        /// Position of function or callback name
        /// </summary>
        public Position NamePosition { get; set; } = new();

        /// <summary>
        /// Position of prefix end function or end callback keyword (="end")
        /// </summary>
        public Position EndKeywordPosition { get; set; } = new();

        /// <summary>
        /// Position of end function or end callback keyword ("on", "function", etc.)
        /// </summary>
        public Position EndOnKeywordPosition { get; set; } = new();

        /// <summary>
        /// Statements, expressions
        /// </summary>
        public AstBlockNode Block { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        protected AstFunctionalNode(
            AstNodeId id,
            IAstNode parent )
            : base( id, parent )
        {
            Block = new AstBlockNode( this );
        }

        #region INameable
        ///
        /// <inheritdoc/>
        ///
        public string Name { get; set; } = string.Empty;
        #endregion INameable

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public override int ChildNodeCount
            => 1;

        ///
        /// <inheritdoc/>
        ///
        public override void AcceptChildren( IAstVisitor visitor )
        {
            Block.AcceptChildren( visitor );
        }
        #endregion IAstNodeAcceptor
    }
}
