using System.Collections.Generic;

namespace KSPCompiler.Domain.Symbols;

public class CommandArgumentSymbol : VariableSymbol
{
    public IReadOnlyList<string> UITypeNames { get; }

    public CommandArgumentSymbol() : this( new List<string>() ) {}

    public CommandArgumentSymbol( IReadOnlyList<string> uiTypeNames )
    {
        UITypeNames = uiTypeNames;
    }
}
