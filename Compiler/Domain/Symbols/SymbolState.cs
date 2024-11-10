namespace KSPCompiler.Domain.Symbols
{
    /// <summary>
    /// Represents the state of a symbol in semantic analysis.
    /// </summary>
    public enum SymbolState
    {
        /// <summary>
        /// Uninitialized
        /// </summary>
        UnInitialized,

        /// <summary>
        /// Initialized
        /// </summary>
        Initialized,

        /// <summary>
        /// In reference
        /// </summary>
        Loading,

        /// <summary>
        /// Referenced
        /// </summary>
        Loaded
    }
}
