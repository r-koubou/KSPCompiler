using System;

using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.CompilerMessages;

/// <summary>
/// Base interface for factories that generate messages.
/// </summary>
/// <remarks>
/// In most cases, it is sufficient to use the default implementation obtained with the <see cref="Default"/> property,
/// but if you need your own implementation, implement this interface.
/// </remarks>
public interface ICompilerMessageFactory
{
    /// <summary>
    /// The default implementation of <see cref="ICompilerMessageFactory"/>.
    /// </summary>
    public static ICompilerMessageFactory Default => new DefaultImpl();

    #region Complex

    /// <summary>
    /// Creates a new message using the specified information.
    /// </summary>
    public CompilerMessage Create( CompilerMessageLevel level, string message, int lineNo = -1, int column = -1, Exception? exception = null );
    #endregion

    #region Simple
    /// <summary>
    /// Creates a new message using the specified information.
    /// </summary>
    /// <remarks>
    /// Simple method for creating <see cref="CompilerMessageLevel.Info"/> level messages.
    /// </remarks>
    public CompilerMessage Info( string message, int lineNo = -1, int column = -1 );

    /// <summary>
    /// Creates a new message using the specified information.
    /// </summary>
    /// <remarks>
    /// Simple method for creating <see cref="CompilerMessageLevel.Warning"/> level messages.
    /// </remarks>
    public CompilerMessage Warning( string message, int lineNo = -1, int column = -1 );

    /// <summary>
    /// Creates a new message using the specified information.
    /// </summary>
    /// <remarks>
    /// Simple method for creating <see cref="CompilerMessageLevel.Error"/> level messages.
    /// </remarks>
    public CompilerMessage Error( string message, int lineNo = -1, int column = -1, Exception? exception = null );

    /// <summary>
    /// Creates a new message using the specified information.
    /// </summary>
    /// <remarks>
    /// Simple method for creating <see cref="CompilerMessageLevel.Fatal"/> level messages.
    /// </remarks>
    public CompilerMessage Fatal( string message, int lineNo = -1, int column = -1, Exception? exception = null );
    #endregion

    private class DefaultImpl : ICompilerMessageFactory
    {
        public CompilerMessage Create( CompilerMessageLevel level, string message, int lineNo = -1, int column = -1, Exception? exception = null )
        {
            var position = new Position
            {
                BeginLine   = lineNo,
                EndLine     = lineNo,
                BeginColumn = column,
                EndColumn   = column
            };

            return new CompilerMessage( level, message, position );
        }

        public CompilerMessage Info( string message, int lineNo = -1, int column = -1 )
            => Create( CompilerMessageLevel.Info, message, lineNo, column );

        public CompilerMessage Warning( string message, int lineNo = -1, int column = -1 )
            => Create( CompilerMessageLevel.Warning, message, lineNo, column );

        public CompilerMessage Error( string message, int lineNo = -1, int column = -1, Exception? exception = null )
            => Create( CompilerMessageLevel.Error, message, lineNo, column, exception );

        public CompilerMessage Fatal( string message, int lineNo = -1, int column = -1, Exception? exception = null )
            => Create( CompilerMessageLevel.Fatal, message, lineNo, column, exception );
    }
}
