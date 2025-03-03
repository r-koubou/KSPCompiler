namespace KSPCompiler.Features.Compilation.Domain.Messages;

/// <summary>
/// Formatter for console output.
/// </summary>
public class ConsoleCompilationMessageFormatter : ICompilationMessageFormatter
{
    public string Format( CompilationMessage message )
    {
        var position = $"{message.Position.BeginLine}:{message.Position.BeginColumn.Value + 1}";

        return $"{message.Level}\t{position}\t{message.Text}";
    }
}
