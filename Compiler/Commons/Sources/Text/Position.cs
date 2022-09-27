namespace KSPCompiler.Commons.Text
{
    public struct Position
    {
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
}
