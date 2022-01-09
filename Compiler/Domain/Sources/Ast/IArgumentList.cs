using KSPCompiler.Domain.Ast.Blocks;

namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// Declaration of nodes containing arguments.
    /// </summary>
    public interface IArgumentList
    {
        /// <summary>
        /// 引数
        /// </summary>
        public AstArgumentList ArgumentList { get; }

        /// <summary>
        /// Whether one or more arguments are stored in the ArgumentList or not.
        /// </summary>
        public virtual bool HasArgument => ArgumentList is { HasArgument: true };

        /// <summary>
        /// Number of arguments stored in ArgumentList, 0 for null.
        /// </summary>
        public virtual int ArgumentCount => HasArgument ? ArgumentList.Arguments.Count : 0;
    }
}
