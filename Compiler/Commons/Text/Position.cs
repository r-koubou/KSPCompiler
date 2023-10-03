namespace KSPCompiler.Commons.Text
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public struct Position
    {
        /// <summary>
        /// Starting line number.
        /// </summary>
        public LineNumber BeginLine { get; init; }

        /// <summary>
        /// End Line Number. It's -1 if it's unknown.
        /// </summary>
        public LineNumber EndLine { get; init; }

        /// <summary>
        /// Start column number.
        /// </summary>
        public Column BeginColumn { get; init; }

        /// <summary>
        /// End Column Number. It's -1 if it's unknown.
        /// </summary>
        public Column EndColumn { get; init; }
    }
    // ReSharper restore UnusedAutoPropertyAccessor.Global
}
