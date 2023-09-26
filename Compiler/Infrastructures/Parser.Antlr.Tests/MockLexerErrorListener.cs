using System.Collections.Generic;
using System.IO;

using Antlr4.Runtime;

namespace KSPCompiler.Parser.Antlr.Tests;

public class MockLexerErrorListener : IAntlrErrorListener<int>
{
    private readonly List<string> _messages = new();
    public IReadOnlyCollection<string> Messages => new List<string>( _messages );

    public bool HasError => _messages.Count > 0;

    public void SyntaxError( TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e )
    {
        string message = $"[Lexer Error] {msg} at line {line}:{charPositionInLine}";
        _messages.Add( message );
        output.WriteLine( message );
    }
}