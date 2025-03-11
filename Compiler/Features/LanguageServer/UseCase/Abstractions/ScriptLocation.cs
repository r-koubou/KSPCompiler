using KSPCompiler.Shared.ValueObjects;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions;

public sealed record ScriptLocation( string Value ) : StringValueObject( Value ) {
    public static ScriptLocation Empty => new( string.Empty );
    public override bool AllowEmpty => true;
}
