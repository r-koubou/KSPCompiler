using System;
using System.IO;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes.Extensions;

namespace KSPCompiler.Domain.Ast.Nodes
{
    /// <summary>
    /// Default implementation of <see cref="IAstNode"/>
    /// </summary>
    public abstract class AstNode : IAstNode, ICloneable
    {
        #region IAstNode
        ///
        /// <inheritdoc/>
        ///
        public virtual AstNodeId Id { get; }

        private Position position = Position.Zero;

        ///
        /// <inheritdoc/>
        ///
        public virtual Position Position
        {
            get => position;
            set => position = value;
        }

        private IAstNode parent;

        ///
        /// <inheritdoc/>
        ///
        public virtual IAstNode Parent
        {
            get => parent;
            set => parent = value;
        }
        #endregion IAstNode

        /// <summary>
        /// Ctor
        /// </summary>
        public AstNode() : this( AstNodeId.None, NullAstNode.Instance ) {}

        /// <summary>
        /// Ctor
        /// </summary>
        public AstNode( AstNodeId id )
        {
            Id     = id;
            parent = NullAstNode.Instance;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AstNode( AstNodeId id, IAstNode parent )
        {
            Id          = id;
            this.parent = parent;
        }

        #region IAstNodeAcceptor
        ///
        /// <inheritdoc />
        ///
        public abstract int ChildNodeCount { get; }

        ///
        /// <inheritdoc/>
        ///
        public abstract IAstNode Accept( IAstVisitor visitor );

        ///
        /// <inheritdoc/>
        ///
        public abstract void AcceptChildren( IAstVisitor visitor );
        #endregion IAstNodeAcceptor

        #region IAstNode

        ///
        /// <inheritdoc/>
        ///
        public TNode GetParent<TNode>() where TNode : IAstNode
        {
            if( !TryGetParent( out TNode result ) )
            {
                throw new NotFoundParentAstNodeException( typeof( TNode ) );
            }

            return result!;
        }

        ///
        /// <inheritdoc/>
        ///
        public bool TryGetParent<TNode>( out TNode result ) where TNode : IAstNode
        {
            result = default!;

            var parentNode = Parent;

            do
            {
                if( parentNode is not TNode targetNode )
                {
                    continue;
                }

                result = targetNode;

                return true;

            } while( ( parentNode = parentNode.Parent ).IsNotNull() );

            return false;
        }

        ///
        /// <inheritdoc/>
        ///
        public bool HasParent<TNode>() where TNode : IAstNode
            => TryGetParent( out TNode _ );

        ///
        /// <inheritdoc/>
        ///
        public virtual void Dump( StreamWriter writer, int indentDepth = 0 )
        {
            if( indentDepth > 0 )
            {
                writer.Write( CreateDumpIndent( indentDepth ) );
            }
            writer.WriteLine( ToString() );
        }

        ///
        /// <inheritdoc/>
        ///
        public virtual void DumpAll( StreamWriter writer, int indentDepth = 0 )
        {
            Dump( writer, indentDepth );
        }

        /// <summary>
        /// クラス名を返す
        /// </summary>
        public override string ToString()
        {
            return nameof( AstNode );
        }

        /// <summary>
        /// ダンプ用の文字列のインデントを挿入する
        /// </summary>
        protected string CreateDumpIndent( int depth )
        {
            string indent = "";

            for( int i = 0; i < depth; i++ )
            {
                indent += "  ";
            }
            return indent;
        }
        #endregion IAstNode methods

        #region ICloneable methods
        /// <summary>
        /// <seealso cref="System.ICloneable.Clone()"/>
        /// </summary>
        object ICloneable.Clone()
        {
            return this.Clone<object>();
        }

        /// <summary>
        /// <seealso cref="System.ICloneable.Clone()"/>
        /// </summary>
        public T Clone<T>() where T : class
        {
            return (T)MemberwiseClone();
        }
        #endregion
    }
}
