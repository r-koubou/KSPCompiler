namespace KSPCompiler.Features.Compilation.Domain.Symbols
{
    /// <summary>
    /// Represents the state of a symbol for evaluation in analysis.
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
