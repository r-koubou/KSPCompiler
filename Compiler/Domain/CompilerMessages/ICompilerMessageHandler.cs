namespace KSPCompiler.Domain.CompilerMessages;

public interface ICompilerMessageHandler
{
    public static readonly ICompilerMessageHandler Empty = new EmptyImpl();

    public void Handle( CompilerMessage message );

    private class EmptyImpl : ICompilerMessageHandler
    {
        public void Handle( CompilerMessage message )
        {}
    }
}