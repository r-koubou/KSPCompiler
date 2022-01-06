#nullable disable

using System;
using System.IO;

using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// Representation of AST node tree.
    /// </summary>
    public interface IAstNode : ICloneable
    {
        public static bool IsNone( IAstNode n ) =>
            n == null ||
            n == None ||
            n is NullNode ||
            n.Id == AstNodeId.None;

        public static IAstNode None => new NullNode();

        /// <summary>
        /// ID for identifying a node.
        /// </summary>
        public virtual AstNodeId Id => AstNodeId.None;

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
    }

    #region Null Object
    public class NullNode : IAstNode
    {
        public object Clone() => new NullNode();
        public AstNodeId Id => AstNodeId.None;
        public Position Position { get; set; } = new ();
        public IAstNode Parent { get; set; } = default!;
    }
    #endregion

}
