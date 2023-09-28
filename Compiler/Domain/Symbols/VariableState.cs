namespace KSPCompiler.Domain.Ast.Symbols
{
    /// <summary>
    /// Represents the state of a variable.
    /// </summary>
    public enum VariableState
    {
        /// <summary>
        /// Uninitialized
        /// </summary>
        UnInitialized,

        /// <summary>
        /// In reference
        /// </summary>
        Loading,

        /// <summary>
        /// Referenced
        /// </summary>
        Loaded,

        /// <summary>
        /// Initialized
        /// </summary>
        Initialized,
    }
}
