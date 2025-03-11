namespace KSPCompiler.Features.Compilation.Domain.Messages;

/// <summary>
/// The event handler in <see cref="ICompilerMessageManger"/> processes the message.
/// </summary>
public interface ICompilerMessageHandler
{
    /// <summary>
    /// Empty implementation. Does nothing.
    /// </summary>
    public static readonly ICompilerMessageHandler Empty = new EmptyImpl();

    /// <summary>
    /// When a message is appended, this method is called.
    /// </summary>
    /// <seealso cref="ICompilerMessageManger.Append"/>
    public void OnAppended( CompilerMessage message );

    private class EmptyImpl : ICompilerMessageHandler
    {
        public void OnAppended( CompilerMessage message )
        {}
    }
}
