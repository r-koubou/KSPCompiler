namespace KSPCompiler.Domain.CompilerMessages;

/// <summary>
/// Base interface for making the final output format of the message body arbitrarily configurable.
/// </summary>
public interface ICompilerMessageFormatter
{
    /// <summary>
    /// Format the message body.
    /// </summary>
    /// <returns>Formatted message body</returns>
    public string Format( CompilerMessage message );
}
