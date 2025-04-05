using System;

namespace KSPCompiler.Shared.Text
{
    // ReSharper disable InconsistentNaming
    public struct Position() : IEquatable<Position>
    {
        public static Position Zero { get; } = new()
        {
            BeginLine   = 0,
            EndLine     = 0,
            BeginColumn = 0,
            EndColumn   = 0
        };

        /// <summary>
        /// Starting line number. (1-based)
        /// </summary>
        public LineNumber BeginLine = 0;

        /// <summary>
        /// End Line Number. It's -1 if it's unknown. (1-based)
        /// </summary>
        public LineNumber EndLine = 0;

        /// <summary>
        /// Start column number. (0-based)
        /// </summary>
        public Column BeginColumn = 0;

        /// <summary>
        /// End Column Number. It's -1 if it's unknown. (0-based)
        /// </summary>
        public Column EndColumn = 0;

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
