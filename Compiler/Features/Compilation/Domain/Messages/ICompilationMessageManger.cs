using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KSPCompiler.Features.Compilation.Domain.Messages;

/// <summary>
/// Base interface for compiler message management during compilation.
/// </summary>
/// <remarks>
/// In most cases, it is sufficient to use the default implementation obtained with the <see cref="Default"/> property,
/// but if you need your own implementation, implement this interface.
/// </remarks>
public interface ICompilationMessageManger
{
    /// <summary>
    /// Create a default implementation of <see cref="ICompilationMessageManger"/>.
    /// </summary>
    public static ICompilationMessageManger Default
        => new DefaultMangerImpl();

    /// <summary>
    /// Stored compiler messages when <see cref="Append(CompilationMessage)"/> is called.
    /// </summary>
    public IReadOnlyCollection<CompilationMessage> Messages { get; }

    /// <summary>
    /// Message factory for creating a new message.
    /// </summary>
    public ICompilationMessageFactory MessageFactory { get; }

    /// <summary>
    /// Message formatter for formatting messages.
    /// </summary>
    public ICompilationMessageFormatter MessageFormatter { get; }

    /// <summary>
    /// Append a new message to the message list.
    /// </summary>
    public void Append( CompilationMessage message );

    /// <summary>
    /// Subscribe this manager to receive messages.
    /// </summary>
    public void AddHandler( ICompilationMessageHandler handler );

    /// <summary>
    /// Unsubscribe this manager from receiving messages.
    /// </summary>
    public void RemoveHandler( ICompilationMessageHandler handler );

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
    public int Count( CompilationMessageLevel level );

    /// <summary>
    /// true if there are no messages stored.
    /// </summary>
    public bool IsEmpty();

    #region Default
    private class DefaultMangerImpl : ICompilationMessageManger
    {
        private readonly List<CompilationMessage> messages = new(256);

        public IReadOnlyCollection<CompilationMessage> Messages
            => new List<CompilationMessage>( messages );

        public ICompilationMessageFactory MessageFactory
            => ICompilationMessageFactory.Default;

        public ICompilationMessageFormatter MessageFormatter
            => new ConsoleCompilationMessageFormatter();

        private List<ICompilationMessageHandler> Handlers { get; } = new();

        public void Append( CompilationMessage message )
        {
            messages.Add( message );

            foreach( var x in Handlers )
            {
                x.OnAppended( message );
            }
        }

        public void AddHandler( ICompilationMessageHandler handler )
        {
            if( !Handlers.Contains( handler ) )
            {
                Handlers.Add( handler );
            }
        }

        public void RemoveHandler( ICompilationMessageHandler handler )
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

        public int Count( CompilationMessageLevel level )
            => messages.Count( x => x.Level == level );

        public bool IsEmpty()
            => messages.Count == 0;
    }
    #endregion
}
