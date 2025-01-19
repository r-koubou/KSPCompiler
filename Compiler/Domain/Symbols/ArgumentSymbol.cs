using System.Collections.Generic;

namespace KSPCompiler.Domain.Symbols;

public abstract record ArgumentSymbol : VariableSymbol
{
    public virtual IReadOnlyList<string> UITypeNames { get; }
    public virtual IReadOnlyList<string> OtherTypeNames { get; }

    protected ArgumentSymbol()
        : this( [] ) {}

    protected ArgumentSymbol( IReadOnlyList<string> uiTypeNames, IReadOnlyList<string> otherTypeNames )
    {
        UITypeNames    = new List<string>( uiTypeNames );
        OtherTypeNames = new List<string>( otherTypeNames );
    }

    protected ArgumentSymbol( IReadOnlyList<string> uiTypeNames )
        : this( uiTypeNames, [] ) {}
}
