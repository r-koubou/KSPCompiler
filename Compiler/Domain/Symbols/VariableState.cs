namespace KSPCompiler.Domain.Symbols
{
    /// <summary>
    /// Represents the state of a variable in semantic analysis.
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
