using System;

using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Domain.CompilerMessages;

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
    public CompilerMessage Create( CompilerMessageLevel level, Position position, string message, Exception? exception = null );

    /// <summary>
    /// Creates a new message using the specified information.
    /// </summary>
    public CompilerMessage Create( CompilerMessageLevel level, int lineNo, int column, string message, Exception? exception = null );

    private class DefaultImpl : ICompilerMessageFactory
    {
        public CompilerMessage Create( CompilerMessageLevel level, Position position, string message, Exception? exception = null )
            => new CompilerMessage( level, message, position );

        public CompilerMessage Create( CompilerMessageLevel level, int lineNo, int column, string message, Exception? exception = null )
        {
            var position = new Position
            {
                BeginLine   = lineNo,
                EndLine     = lineNo,
                BeginColumn = column,
                EndColumn   = column
            };

            return Create( level, position, message, exception );
        }
    }
}
