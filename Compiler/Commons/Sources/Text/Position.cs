namespace KSPCompiler.Commons.Text
{
    public class Position
    {
        /// <summary>
        /// Starting line number.
        /// </summary>
        public LineNumber BeginLine = 0;
        /// <summary>
        /// End Line number.
        /// </summary>
        public LineNumber EndLine = LineNumber.Unknown;
        /// <summary>
        /// Start column number.
        /// </summary>
        public Column BeginColumn = 0;
        /// <summary>
        /// End Column number.
        /// </summary>
        public Column EndColumn = Column.Unknown;
    }
}
