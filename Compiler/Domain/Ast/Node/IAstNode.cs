using System.IO;

using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast.Node
{
    /// <summary>
    /// Representation of AST node tree.
    /// </summary>
    public interface IAstNode
    {
        public static bool IsNone( IAstNode? n ) =>
            n == null ||
            n.Id == AstNodeId.None;

        /// <summary>
        /// ID for identifying a node.
        /// </summary>
        public AstNodeId Id => AstNodeId.None;

        /// <summary>
        /// Token location information.
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// Parent node. If not present, the value is null.
        /// </summary>
        public IAstNode? Parent { get; set; }

        /// <summary>
        /// Retrieves the parent node with the specified TNode.
        /// </summary>
        bool TryGetParent<TNode>( out TNode? result ) where TNode : IAstNode;

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
    }
}
