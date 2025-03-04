using System.Collections.Generic;

using KSPCompiler.Shared.Text;

namespace KSPCompiler.Shared.Domain.Ast.Nodes.Blocks
{
    /// <summary>
    /// AST node representing a callback definition
    /// </summary>
    public class AstCallbackDeclarationNode : AstFunctionalNode, IArgumentList
    {
        /// <summary>
        /// Comment lines if written above the variable declaration, otherwise empty
        /// </summary>
        public IReadOnlyCollection<string> CommentLines { get; set; } = new List<string>();

        /// <summary>
        /// Callback name position
        /// </summary>
        public Position CallbackNamePosition { get; set; } = new();

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallbackDeclarationNode() : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstCallbackDeclarationNode( IAstNode parent )
            : base( AstNodeId.CallbackDeclaration, parent )
        {
            ArgumentList = new AstArgumentListNode( this );
        }

        #region IArgument

        ///
        /// <inheritdoc />
        ///
        public AstArgumentListNode ArgumentList { get; set; }

        #endregion IArgument

        #region IAstNodeAcceptor

        ///
        /// <inheritdoc/>
        ///
        public override IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );


        #endregion IAstNodeAcceptor
    }
}
