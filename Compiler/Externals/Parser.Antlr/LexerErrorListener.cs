using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Externals.Parser.Antlr;

internal class LexerErrorListener : IAntlrErrorListener<int>
{
    private ICompilerMessageManger MessageManger { get; }
    public bool HasError { get; private set; }

    public LexerErrorListener( ICompilerMessageManger messageManger )
    {
        MessageManger = messageManger;
    }

    public void SyntaxError( TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e )
    {
        HasError = true;

        var factory = ICompilerMessageFactory.Default;
        var compilerMessage = factory.Create(
            CompilerMessageLevel.Error,
            msg,
            line,
            charPositionInLine
        );

        MessageManger.Append( compilerMessage );
    }
}