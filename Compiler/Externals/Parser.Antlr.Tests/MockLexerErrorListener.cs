using System.IO;

using Antlr4.Runtime;

namespace KSPCompiler.Parser.Antlr.Tests;

public class MockLexerErrorListener : IAntlrErrorListener<int>
{
    public bool HasError { get; private set; }
    public void SyntaxError( TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e )
    {
        HasError = true;
        output.WriteLine( $"[Lexer Error] {msg} at line {line}:{charPositionInLine}" );
    }
}