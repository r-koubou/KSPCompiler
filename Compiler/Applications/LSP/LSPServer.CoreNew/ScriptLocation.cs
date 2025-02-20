using KSPCompiler.Commons.ValueObjects;

namespace KSPCompiler.Applications.LSPServer.CoreNew;

public sealed record ScriptLocation( string Value ) : StringValueObject( Value ) {
    public static ScriptLocation Empty => new( string.Empty );
    public override bool AllowEmpty => true;
}
