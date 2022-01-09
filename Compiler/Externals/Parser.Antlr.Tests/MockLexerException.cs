using System.Collections.Generic;

namespace KSPCompiler.Parser.Antlr.Tests;

public class MockLexerException : System.Exception
{
    private IReadOnlyCollection<string> Messages { get; }

    public MockLexerException( IReadOnlyCollection<string> messages )
    {
        Messages = messages;
    }
}