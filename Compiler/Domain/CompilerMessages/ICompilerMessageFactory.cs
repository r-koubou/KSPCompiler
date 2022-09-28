using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.CompilerMessages;

public interface ICompilerMessageFactory
{
    public static ICompilerMessageFactory Default => new DefaultImpl();

    public CompilerMessage Create( CompilerMessageLevel level, string message, int lineNo = -1, int column = -1 );

    private class DefaultImpl : ICompilerMessageFactory
    {
        public CompilerMessage Create( CompilerMessageLevel level, string message, int lineNo = -1, int column = -1 )
        {
            var position = new Position
            {
                BeginLine   = lineNo,
                EndLine     = lineNo,
                BeginColumn = column,
                EndColumn   = column
            };

            return new CompilerMessage( level, message, position );
        }
    }
}
