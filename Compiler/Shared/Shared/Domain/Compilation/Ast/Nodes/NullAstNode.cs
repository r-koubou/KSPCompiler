﻿using KSPCompiler.Shared.Text;

namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes
{
    /// <summary>
    /// Null Object of <see cref="IAstNode"/>
    /// </summary>
    public sealed class NullAstNode : IAstNode
    {
        /// <summary>
        /// The Null Object instance.
        /// </summary>
        public static readonly IAstNode Instance = new NullAstNode();

        /// <summary>
        /// Always return <see cref="AstNodeId.None"/>.
        /// </summary>
        public AstNodeId Id
            => AstNodeId.None;

        /// <summary>
        /// Always return zero and the set is ignored.
        /// </summary>
        public Position Position
        {
            get => Position.Zero;
            set => _ = value;
        }

        /// <summary>
        /// Always return this instance and the set is ignored.
        /// </summary>
        public IAstNode Parent
        {
            get => Instance;
            set => _ = value;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        private NullAstNode() {}

        /// <summary>
        /// Always throw <see cref="NotFoundParentAstNodeException"/>.
        /// </summary>
        public TNode GetParent<TNode>() where TNode : IAstNode
            => throw new NotFoundParentAstNodeException( typeof( TNode ) );

        /// <summary>
        /// Always return false and the out parameter is default.
        /// </summary>
        public bool TryGetParent<TNode>( out TNode result ) where TNode : IAstNode
        {
            result = default!;
            return false;
        }

        /// <summary>
        /// Always return false.
        /// </summary>
        public bool HasParent<TNode>() where TNode : IAstNode
            => false;

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public int ChildNodeCount
            => 0;

        ///
        /// <inheritdoc/>
        ///
        public IAstNode Accept( IAstVisitor visitor )
            => visitor.Visit( this );

        /// <summary>
        /// Do nothing.
        /// </summary>
        public void AcceptChildren( IAstVisitor visitor ) {}
        #endregion IAstNodeAcceptor

        public override string ToString()
            => nameof( NullAstNode );
    }
}
