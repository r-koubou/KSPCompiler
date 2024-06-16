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

    /// <summary>
    /// Creates a new message using the specified information.
    /// </summary>
    public CompilerMessage Create( CompilerMessageLevel level, string message, int lineNo = -1, int column = -1, Exception? exception = null );

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
    }
}
