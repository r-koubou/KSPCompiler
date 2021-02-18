using System;
using System.IO;

using KSPCompiler.Domain.TextFile.Aggregate;

namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// Default implementation of <see cref="IAstNode"/>
    /// </summary>
    public abstract class AstNode : IAstNode, IAstNodeAcceptor
    {
        #region IAstNode
        ///
        /// <inheritdoc/>
        ///
        public AstNodeId Id { get; }

        ///
        /// <inheritdoc/>
        ///
        public Position Position { get; set; } = new Position();

        ///
        /// <inheritdoc/>
        ///
        public IAstNode Parent { get; set; }
        #endregion IAstNode

        /// <summary>
        /// Ctor
        /// </summary>
        public AstNode( AstNodeId id, IAstNode parent )
        {
            Id     = id;
            Parent = parent;
        }

        #region IASTNodeAcceptor
        ///
        /// <inheritdoc/>
        ///
        public abstract T Accept<T>( IAstVisitor<T> visitor );

        ///
        /// <inheritdoc/>
        ///
        public abstract void AcceptChildren<T>( IAstVisitor<T> visitor );
        #endregion IASTNodeAcceptor

        #region IAstNode
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
            return nameof(AstNode);
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
        #endregion IASTNode methods

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
