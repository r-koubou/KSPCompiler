using System.Collections.Generic;

namespace KSPCompiler.Domain.Symbols;

public sealed record CommandArgumentSymbol : ArgumentSymbol
{
    public CommandArgumentSymbol() {}

    public CommandArgumentSymbol( IReadOnlyList<string> uiTypeNames, IReadOnlyList<string> otherTypeNames )
        : base( uiTypeNames, otherTypeNames ) {}

    public CommandArgumentSymbol( IReadOnlyList<string> uiTypeNames )
        : base( uiTypeNames ) {}
}
