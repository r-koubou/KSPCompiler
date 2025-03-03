namespace KSPCompiler.Features.Compilation.Domain.Messages;

/// <summary>
/// Base interface for making the final output format of the message body arbitrarily configurable.
/// </summary>
public interface ICompilationMessageFormatter
{
    /// <summary>
    /// Format the message body.
    /// </summary>
    /// <returns>Formatted message body</returns>
    public string Format( CompilationMessage message );
}
