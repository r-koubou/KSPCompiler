using System;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class KspParseException : Exception
{
    public KspParseException( string message ) : base( message )
    {}
}
