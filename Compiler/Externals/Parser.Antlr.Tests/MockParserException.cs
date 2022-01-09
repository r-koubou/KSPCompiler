using System.Collections.Generic;

namespace KSPCompiler.Parser.Antlr.Tests;

public class MockParserException : System.Exception
{
    private IReadOnlyCollection<string> Messages { get; }

    public MockParserException( IReadOnlyCollection<string> messages )
    {
        Messages = messages;
    }
}