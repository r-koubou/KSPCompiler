namespace KSPCompiler.Domain.CompilerMessages;

public class ConsoleCompilerMessageFormatter : ICompilerMessageFormatter
{
    public string Format( CompilerMessage message )
    {
        var position = $"{message.Position.BeginLine}:{message.Position.BeginColumn.Value + 1}";

        return $"{message.Level}\t{position}\t{message.Text}";
    }
}
