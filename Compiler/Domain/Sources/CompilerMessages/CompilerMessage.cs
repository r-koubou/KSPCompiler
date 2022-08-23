using System;

using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.CompilerMessages;

public class CompilerMessage
{
    public DateTime DateTime { get; }
    public CompilerMessageLevel Level { get; }
    public CompilerMessageText Text { get; }
    public Position Position { get; }

    public CompilerMessage( CompilerMessageLevel level, CompilerMessageText text, Position position )
    {
        DateTime = DateTime.Now;
        Level    = level;
        Text     = text;
        Position = position;
    }
}