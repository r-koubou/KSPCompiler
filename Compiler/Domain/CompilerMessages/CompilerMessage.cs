using System;

using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.CompilerMessages;

public class CompilerMessage
{
    public DateTime DateTime { get; }
    public CompilerMessageLevel Level { get; }
    public CompilerMessageText Text { get; }
    public Position Position { get; }
    public Exception? Exception { get; }

    public CompilerMessage( CompilerMessageLevel level, CompilerMessageText text, Position position, Exception? exception = null )
    {
        DateTime  = DateTime.Now;
        Level     = level;
        Text      = text;
        Position  = position;
        Exception = exception;
    }
}
