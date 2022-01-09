using System.Collections.Generic;
using System.IO;

using Antlr4.Runtime;

namespace KSPCompiler.Parser.Antlr.Tests;

public class MockParserErrorListener : IAntlrErrorListener<IToken>
{
    private readonly List<string> _messages = new();
    public IReadOnlyCollection<string> Messages => new List<string>( _messages );

    public bool HasError => _messages.Count > 0;

    public void SyntaxError( TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e )
    {
        string message = $"[Parser Error] {msg} at line {line}:{charPositionInLine}";
        _messages.Add( message );
        output.WriteLine( message );
    }
}