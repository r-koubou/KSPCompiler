using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KSPCompiler.Features.Compilation.Domain.CompilerMessages;

/// <summary>
/// Base interface for compiler message management during compilation.
/// </summary>
/// <remarks>
/// In most cases, it is sufficient to use the default implementation obtained with the <see cref="Default"/> property,
/// but if you need your own implementation, implement this interface.
/// </remarks>
public interface ICompilerMessageManger
{
    /// <summary>
    /// Create a default implementation of <see cref="ICompilerMessageManger"/>.
    /// </summary>
    public static ICompilerMessageManger Default
        => new DefaultMangerImpl();

    /// <summary>
    /// Stored compiler messages when <see cref="Append(CompilerMessage)"/> is called.
    /// </summary>
    public IReadOnlyCollection<CompilerMessage> Messages { get; }

    /// <summary>
    /// Message factory for creating a new message.
    /// </summary>
    public ICompilerMessageFactory MessageFactory { get; }

    /// <summary>
    /// Message formatter for formatting messages.
    /// </summary>
    public ICompilerMessageFormatter MessageFormatter { get; }

    /// <summary>
    /// Append a new message to the message list.
    /// </summary>
    public void Append( CompilerMessage message );

    /// <summary>
    /// Subscribe this manager to receive messages.
    /// </summary>
    public void AddHandler( ICompilerMessageHandler handler );

    /// <summary>
    /// Unsubscribe this manager from receiving messages.
    /// </summary>
    public void RemoveHandler( ICompilerMessageHandler handler );

    /// <summary>
    /// Write all messages to the specified writer.
    /// </summary>
    public void WriteTo( TextWriter writer );

    /// <summary>
    /// Get the number of all messages stored.
    /// </summary>
    public int Count();

    /// <summary>
    /// Get the number of messages with the specified level.
    /// </summary>
    public int Count( CompilerMessageLevel level );

    /// <summary>
    /// true if there are no messages stored.
    /// </summary>
    public bool IsEmpty();

    #region Default
    private class DefaultMangerImpl : ICompilerMessageManger
    {
        private readonly List<CompilerMessage> messages = new(256);

        public IReadOnlyCollection<CompilerMessage> Messages
            => new List<CompilerMessage>( messages );

        public ICompilerMessageFactory MessageFactory
            => ICompilerMessageFactory.Default;

        public ICompilerMessageFormatter MessageFormatter
            => new ConsoleCompilerMessageFormatter();

        private List<ICompilerMessageHandler> Handlers { get; } = new();

        public void Append( CompilerMessage message )
        {
            messages.Add( message );

            foreach( var x in Handlers )
            {
                x.OnAppended( message );
            }
        }

        public void AddHandler( ICompilerMessageHandler handler )
        {
            if( !Handlers.Contains( handler ) )
            {
                Handlers.Add( handler );
            }
        }

        public void RemoveHandler( ICompilerMessageHandler handler )
        {
            if( Handlers.Contains( handler ) )
            {
                Handlers.Remove( handler );
            }
        }

        public void WriteTo( TextWriter writer )
        {
            foreach( var x in Messages )
            {
                writer.WriteLine( MessageFormatter.Format( x ) );
            }
        }

        public int Count()
            => messages.Count;

        public int Count( CompilerMessageLevel level )
            => messages.Count( x => x.Level == level );

        public bool IsEmpty()
            => messages.Count == 0;
    }
    #endregion
}
