using System;
using System.Collections.Generic;

namespace KSPCompiler.Features.Compilation.Domain.Symbols;

public abstract class ArgumentSymbol : VariableSymbol
{
    public virtual IReadOnlyList<string> UITypeNames { get; }
    public virtual IReadOnlyList<string> OtherTypeNames { get; }

    protected ArgumentSymbol()
        : this( Array.Empty<string>(), Array.Empty<string>() ) {}

    protected ArgumentSymbol( IReadOnlyList<string> uiTypeNames, IReadOnlyList<string> otherTypeNames )
    {
        UITypeNames    = new List<string>( uiTypeNames );
        OtherTypeNames = new List<string>( otherTypeNames );
    }

    protected ArgumentSymbol( IReadOnlyList<string> uiTypeNames )
        : this( uiTypeNames, Array.Empty<string>() ) {}
}
