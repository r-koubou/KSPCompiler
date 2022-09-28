using System.Collections.Generic;

namespace KSPCompiler.Domain.CompilerMessages;

public interface ICompilerMessageManger
{
    public IReadOnlyCollection<CompilerMessage> Messages { get; }
    public ICompilerMessageFactory MessageFactory { get; }

    public void Append( CompilerMessage message );
    public void AddHandler( ICompilerMessageHandler handler );
    public void RemoveHandler( ICompilerMessageHandler handler );

    public static ICompilerMessageManger CreateDefault()
        => new DefaultMangerImpl();

    #region Default
    private class DefaultMangerImpl : ICompilerMessageManger
    {
        private readonly List<CompilerMessage> _messages = new(256);
        public IReadOnlyCollection<CompilerMessage> Messages
            => new List<CompilerMessage>( _messages );

        public ICompilerMessageFactory MessageFactory
            => ICompilerMessageFactory.Default;

        private List<ICompilerMessageHandler> Handlers { get; } = new();

        public void Append( CompilerMessage message )
        {
            _messages.Add( message );

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
    }
    #endregion
}