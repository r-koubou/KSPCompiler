namespace KSPCompiler.Domain.Ast.Node
{
    /// <summary>
    /// Declaring a constant expressible node.
    /// </summary>
    public interface IConstable
    {
        /// <summary>
        /// Returns whether the node is a constant expressible node or not.
        /// </summary>
        public bool IsConstant { get; set; }
    }
}
