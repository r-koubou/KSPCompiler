/* =========================================================================

    IASTNameable.cs
    Copyright (c) R-Koubou

========================================================================= */

namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// Declaration of Individually Named Nodes.
    /// </summary>
    public interface INameable
    {
        /// <summary>
        /// Any name that identifies the node.
        /// </summary>
        public string Name { get; set; }
    }
}
