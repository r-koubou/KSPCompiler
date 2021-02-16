using System.IO;

using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.TextFile.Aggregate;

namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// Representation of AST node tree.
    /// </summary>
    public interface IAstNode : System.ICloneable
    {
        public static IAstNode None => NullNode.Instance;

        /// <summary>
        /// ID for identifying a node.
        /// </summary>
        public AstNodeId Id { get; }

        /// <summary>
        /// Token location information.
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// Parent node. If not present, the value is null.
        /// </summary>
        public IAstNode Parent { get; set; }

        /// <summary>
        /// Dumping information on this node for debugging purposes.
        /// </summary>
        public void Dump( StreamWriter writer, int indentDepth = 0 )
        {}

        /// <summary>
        /// Dump the information of this node and its children for debugging purposes.
        /// </summary>
        public void DumpAll( StreamWriter writer, int indentDepth = 0 )
        {}

        #region Null Object
        private sealed class NullNode : IAstNode
        {
            public static readonly IAstNode Instance = new NullNode();

            private NullNode() {}
            public object Clone() => new NullNode();
            public AstNodeId Id => AstNodeId.None;
            public Position Position { get; set; } = new ();
            public IAstNode Parent { get; set; } = default!;
        }
        #endregion

    }
}
