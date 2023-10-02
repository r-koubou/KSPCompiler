using System;

using KSPCompiler.Commons.Text;

namespace KSPCompiler.Domain.CompilerMessages;

public interface ICompilerMessageFactory
{
    public static ICompilerMessageFactory Default => new DefaultImpl();

    #region Complex
    public CompilerMessage Create( CompilerMessageLevel level, string message, int lineNo = -1, int column = -1, Exception? exception = null );
    #endregion

    #region Simple
    public CompilerMessage Info( string message, int lineNo = -1, int column = -1 );
    public CompilerMessage Warning( string message, int lineNo = -1, int column = -1 );
    public CompilerMessage Error( string message, int lineNo = -1, int column = -1, Exception? exception = null );
    public CompilerMessage Fatal( string message, int lineNo = -1, int column = -1, Exception? exception = null );
    #endregion

    private class DefaultImpl : ICompilerMessageFactory
    {
        public CompilerMessage Create( CompilerMessageLevel level, string message, int lineNo = -1, int column = -1, Exception? exception = null )
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

        public CompilerMessage Info( string message, int lineNo = -1, int column = -1 )
            => Create( CompilerMessageLevel.Info, message, lineNo, column );

        public CompilerMessage Warning( string message, int lineNo = -1, int column = -1 )
            => Create( CompilerMessageLevel.Warning, message, lineNo, column );

        public CompilerMessage Error( string message, int lineNo = -1, int column = -1, Exception? exception = null )
            => Create( CompilerMessageLevel.Error, message, lineNo, column, exception );

        public CompilerMessage Fatal( string message, int lineNo = -1, int column = -1, Exception? exception = null )
            => Create( CompilerMessageLevel.Fatal, message, lineNo, column, exception );
    }
}
