using System;

namespace KSPCompiler.Commons.Text
{
    // ReSharper disable InconsistentNaming
    public struct Position : IEquatable<Position>
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

        public bool Equals( Position other )
            => this == other;

        public override bool Equals( object? obj )
        {
            if( obj is null )
            {
                return false;
            }

            return obj is Position position && Equals( position );
        }

        public override int GetHashCode()
            => HashCode.Combine(
                BeginLine.Value,
                EndLine.Value,
                BeginColumn.Value,
                EndColumn.Value
            );

        #region Operators
        public static bool operator ==(Position a, Position b)
        {
            return a.BeginLine.Value == b.BeginLine.Value
                     && a.EndLine.Value == b.EndLine.Value
                     && a.BeginColumn.Value == b.BeginColumn.Value
                     && a.EndColumn.Value == b.EndColumn.Value;
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }
        #endregion

    }
    // ReSharper restore InconsistentNaming
}
