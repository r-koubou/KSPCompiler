namespace KSPCompiler.Commons.Text
{
    // ReSharper disable InconsistentNaming
    public struct Position
    {
        public static Position Zero { get; } = new()
        {
            BeginLine   = 0,
            EndLine     = 0,
            BeginColumn = 0,
            EndColumn   = 0
        };

        /// <summary>
        /// Starting line number.
        /// </summary>
        public LineNumber BeginLine;

        /// <summary>
        /// End Line Number. It's -1 if it's unknown.
        /// </summary>
        public LineNumber EndLine;

        /// <summary>
        /// Start column number.
        /// </summary>
        public Column BeginColumn;

        /// <summary>
        /// End Column Number. It's -1 if it's unknown.
        /// </summary>
        public Column EndColumn;
    }
    // ReSharper restore InconsistentNaming
}
