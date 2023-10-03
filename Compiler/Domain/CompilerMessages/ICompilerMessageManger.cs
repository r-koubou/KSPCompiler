using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KSPCompiler.Domain.CompilerMessages;

public interface ICompilerMessageManger
{
    public IReadOnlyCollection<CompilerMessage> Messages { get; }
    public ICompilerMessageFactory MessageFactory { get; }
    public ICompilerMessageFormatter MessageFormatter { get; }

    public void Append( CompilerMessage message );
    public void AddHandler( ICompilerMessageHandler handler );
    public void RemoveHandler( ICompilerMessageHandler handler );
    public void WriteTo( TextWriter writer );

    public static ICompilerMessageManger CreateDefault()
        => new DefaultMangerImpl();

    public int Count();
    public int Count( CompilerMessageLevel level );
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
                x.Handle( message );
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
