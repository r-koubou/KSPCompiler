using System;
using System.Collections.Generic;

namespace KSPCompiler.Domain.Ast.Nodes
{
    /// <summary>
    /// Represents a modifier information node.
    /// </summary>
    public class AstModiferNode : AstNode
    {
        /// <summary>
        /// A stored collection of modifier values.
        /// </summary>
        public IReadOnlyCollection<string> Values { get; }

        public override int ChildNodeCount
            => 0;

        public AstModiferNode()
            : this( NullAstNode.Instance, Array.Empty<string>() ) {}

        public AstModiferNode( string modifier )
            : this( NullAstNode.Instance, new[] { modifier } ) {}

        public AstModiferNode( IEnumerable<string> modifiers )
            : this( NullAstNode.Instance, modifiers ) {}

        public AstModiferNode( IAstNode parent, string modifier )
            : this( parent, new[] { modifier } ) {}

        public AstModiferNode( IAstNode parent, IEnumerable<string> modifiers )
            : base( AstNodeId.Modifier, parent )
        {
            Values = new List<string>( modifiers );
        }

        #region IAstNodeAcceptor

        public override T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        public override void AcceptChildren<T>( IAstVisitor<T> visitor ) {}

        #endregion
    }
}
