using KSPCompiler.Domain.Ast.Blocks;

namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// Declaration of nodes containing arguments.
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        /// 引数
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
