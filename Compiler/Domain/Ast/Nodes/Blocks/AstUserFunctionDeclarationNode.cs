using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast.Nodes.Blocks
{
    /// <summary>
    /// AST node representing an user Defined Functions (KSP)
    /// </summary>
    public class AstUserFunctionDeclarationNode : AstFunctionalNode
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public AstUserFunctionDeclarationNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Function name position
        /// </summary>
        public Position FunctionNamePosition { get; set; } = new();

        /// <summary>
        /// Ctor
        /// </summary>
        public AstUserFunctionDeclarationNode( IAstNode parent )
            : base( AstNodeId.UserFunctionDeclaration, parent )
        {
        }

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );


        #endregion IAstNodeAcceptor
    }
}
