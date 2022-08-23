using System;

namespace KSPCompiler.Externals.Parser.Antlr;

public class KspParserException : Exception
{
    public KspParserException( string message ) : base( message )
    {}
}