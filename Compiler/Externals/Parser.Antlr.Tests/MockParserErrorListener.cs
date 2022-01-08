using System.IO;

using Antlr4.Runtime;

namespace KSPCompiler.Parser.Antlr.Tests;

public class MockParserErrorListener : IAntlrErrorListener<IToken>
{
    public bool HasError { get; private set; }
    public void SyntaxError( TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e )
    {
        HasError = true;
        output.WriteLine( $"[Parser Error] {msg} at line {line}:{charPositionInLine}" );
    }
}