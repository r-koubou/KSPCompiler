using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Externals.Parser.Antlr;

internal class ParserErrorListener : IAntlrErrorListener<IToken>
{
    private ICompilerMessageManger MessageManger { get; }

    public bool HasError { get; private set; }

    public ParserErrorListener( ICompilerMessageManger messageManger )
    {
        MessageManger = messageManger;
    }

    public void SyntaxError( TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e )
    {
        HasError = true;

        var compilerMessage = MessageManger.MessageFactory.Create(
            CompilerMessageLevel.Error,
            msg,
            line,
            charPositionInLine
        );

        MessageManger.Append( compilerMessage );
    }
}
