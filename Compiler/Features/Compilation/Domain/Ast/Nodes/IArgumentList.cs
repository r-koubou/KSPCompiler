using KSPCompiler.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Domain.Ast.Nodes
{
    /// <summary>
    /// Declaration of nodes containing arguments.
    /// </summary>
    public interface IArgumentList
    {
        /// <summary>
        /// 引数
        /// </summary>
        public AstArgumentListNode ArgumentList { get; }

        /// <summary>
        /// Whether one or more arguments are stored in the ArgumentList or not.
        /// </summary>
        public bool HasArgument => ArgumentList is { HasArgument: true };

        /// <summary>
        /// Number of arguments stored in ArgumentList, 0 for null.
        /// </summary>
        public int ArgumentCount => HasArgument ? ArgumentList.Arguments.Count : 0;
    }
}
