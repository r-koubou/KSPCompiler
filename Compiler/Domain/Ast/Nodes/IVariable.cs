namespace KSPCompiler.Domain.Ast.Node
{
    /// <summary>
    /// Declarations of value-representable nodes.
    /// </summary>
    public interface IVariable
    {
        /// <summary>
        /// A value.
        /// </summary>
        public object Value { get; set; }
    }

    /// <summary>
    /// Declarations of value-representable nodes (generic version).
    /// </summary>
    public interface IVariable<T>
    {
        /// <summary>
        /// A value.
        /// </summary>
        public T Value { get; set; }
    }
}
