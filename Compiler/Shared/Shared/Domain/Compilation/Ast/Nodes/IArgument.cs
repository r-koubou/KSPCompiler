using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;

namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes
{
    /// <summary>
    /// Declaration of nodes containing arguments.
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        /// Argument list
        /// </summary>
        public AstArgumentListNode ArgumentList { get; set; }

        /// <summary>
        /// Whether one or more arguments are stored in the ArgumentList or not.
        /// </summary>
        public bool HasArgument { get; }

        /// <summary>
        /// Number of arguments stored in ArgumentList, 0 for null.
        /// </summary>
        public int ArgumentCount { get; }
    }
}
