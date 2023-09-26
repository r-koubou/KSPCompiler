using System;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class KspParserException : Exception
{
    public KspParserException( string message ) : base( message )
    {}
}