using System;

namespace KSPCompiler.Domain;

public abstract class KspCompilerException : Exception
{
    protected KspCompilerException( string message ) : base( message )
    {}
}
