namespace KSPCompiler.Features.Compilation.Domain.Messages;

/// <summary>
/// The event handler in <see cref="ICompilationMessageManger"/> processes the message.
/// </summary>
public interface ICompilationMessageHandler
{
    /// <summary>
    /// Empty implementation. Does nothing.
    /// </summary>
    public static readonly ICompilationMessageHandler Empty = new EmptyImpl();

    /// <summary>
    /// When a message is appended, this method is called.
    /// </summary>
    /// <seealso cref="ICompilationMessageManger.Append"/>
    public void OnAppended( CompilationMessage message );

    private class EmptyImpl : ICompilationMessageHandler
    {
        public void OnAppended( CompilationMessage message )
        {}
    }
}
