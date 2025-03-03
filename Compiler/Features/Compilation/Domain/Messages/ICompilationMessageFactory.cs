using System;

using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Compilation.Domain.Messages;

/// <summary>
/// Base interface for factories that generate messages.
/// </summary>
/// <remarks>
/// In most cases, it is sufficient to use the default implementation obtained with the <see cref="Default"/> property,
/// but if you need your own implementation, implement this interface.
/// </remarks>
public interface ICompilationMessageFactory
{
    /// <summary>
    /// The default implementation of <see cref="ICompilationMessageFactory"/>.
    /// </summary>
    public static ICompilationMessageFactory Default => new DefaultImpl();

    /// <summary>
    /// Creates a new message using the specified information.
    /// </summary>
    public CompilationMessage Create( CompilationMessageLevel level, Position position, string message, Exception? exception = null );

    /// <summary>
    /// Creates a new message using the specified information.
    /// </summary>
    public CompilationMessage Create( CompilationMessageLevel level, int lineNo, int column, string message, Exception? exception = null );

    private class DefaultImpl : ICompilationMessageFactory
    {
        public CompilationMessage Create( CompilationMessageLevel level, Position position, string message, Exception? exception = null )
            => new CompilationMessage( level, message, position );

        public CompilationMessage Create( CompilationMessageLevel level, int lineNo, int column, string message, Exception? exception = null )
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
