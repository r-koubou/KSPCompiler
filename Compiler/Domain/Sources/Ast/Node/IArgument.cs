using KSPCompiler.Domain.Ast.Node.Blocks;

namespace KSPCompiler.Domain.Ast.Node
{
    /// <summary>
    /// Declaration of nodes containing arguments.
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        /// Argument list
        /// </summary>
        public AstArgumentList ArgumentList { get; set; }

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
