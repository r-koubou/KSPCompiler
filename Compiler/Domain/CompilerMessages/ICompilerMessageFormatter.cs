namespace KSPCompiler.Domain.CompilerMessages;

public interface ICompilerMessageFormatter
{
    public string Format( CompilerMessage message );
}
