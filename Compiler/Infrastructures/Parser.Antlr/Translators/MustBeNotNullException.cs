using System;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators;

/// <summary>
/// Throws when a variable expected to be not null, but it is null.
/// </summary>
internal class MustBeNotNullException : Exception
{
    public MustBeNotNullException( string name ) : base( $"{name} : must be not null" ) { }
}
