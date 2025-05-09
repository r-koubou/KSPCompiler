using System.Collections.Generic;

using KSPCompiler.Shared.Text;

namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements
{
    /// <summary>
    /// AST node representing a variable declaration
    /// </summary>
    public class AstVariableDeclarationNode : AstStatementNode, INameable
    {
        /// <summary>
        /// Comment lines if written above the variable declaration, otherwise empty
        /// </summary>
        public IReadOnlyCollection<string> CommentLines { get; set; } = new List<string>();

        /// <summary>
        /// modifier
        /// </summary>
        public AstModiferNode Modifier { get; set; } = NullAstModiferNode.Instance;

        #region INamable

        /// <summary>
        /// variable name
        /// </summary>
        public string Name { get; set; } = "";

        #endregion

        /// <summary>
        /// initialization statement
        /// </summary>
        public AstVariableInitializerNode Initializer { get; set; } = NullAstVariableInitializerNode.Instance;

        /// <summary>
        /// Variable name position
        /// </summary>
        public Position VariableNamePosition { get; set; } = new();

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableDeclarationNode()
            : this( NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstVariableDeclarationNode( IAstNode parent )
            : base( AstNodeId.VariableDeclaration, parent ) {}

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
            Modifier.Accept( visitor );
            Initializer.AcceptChildren( visitor );
        }

        #endregion IAstNodeAcceptor
    }
}
