using System.IO;

using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast.Nodes
{
    /// <summary>
    /// Representation of AST node tree.
    /// </summary>
    public interface IAstNode : IAstNodeAcceptor
    {
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
        public IAstNode Parent { get; set; }

        /// <summary>
        /// Retrieves the parent node with the specified TNode.
        /// </summary>
        /// <remarks>
        /// It traverses the upper nodes until it finds one and returns the first node that matches the TNode.
        /// </remarks>
        /// <exception cref="NotFoundParentAstNodeException">if the parent TNode is not found</exception>
        TNode GetParent<TNode>() where TNode : IAstNode;

        /// <summary>
        /// Retrieves the parent node with the specified TNode.
        /// </summary>
        /// <remarks>
        /// It traverses the upper nodes until it finds one and returns the first node that matches the TNode.
        /// </remarks>
        /// <returns>true if the parent TNode is found, otherwise false</returns>
        /// <out>the parent node will be stored if found otherwise default</out>
        bool TryGetParent<TNode>( out TNode result ) where TNode : IAstNode;

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
