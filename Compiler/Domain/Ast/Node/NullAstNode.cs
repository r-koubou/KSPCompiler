using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast.Node
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
        /// Always return zero and the set is ignored.
        /// </summary>
        public Position Position
        {
            get => new ();
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
        /// Always return false and the out parameter is default.
        /// </summary>
        public bool TryGetParent<TNode>( out TNode? result ) where TNode : IAstNode
        {
            result = default;
            return false;
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public T Accept<T>( IAstVisitor<T> visitor )
            => visitor.Visit( this );

        /// <summary>
        /// Do nothing.
        /// </summary>
        public void AcceptChildren<T>( IAstVisitor<T> visitor ) {}
        #endregion IAstNodeAcceptor

        public override string ToString()
            => nameof(NullAstNode);
    }
}
