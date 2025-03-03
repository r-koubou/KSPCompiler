using System;

namespace KSPCompiler.Features.Compilation.Domain;

public abstract class KspCompilerException : Exception
{
    protected KspCompilerException( string message ) : base( message )
    {}
}
