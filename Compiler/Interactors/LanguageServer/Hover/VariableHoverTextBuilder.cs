using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Interactors.LanguageServer.Hover;

public class VariableHoverTextBuilder : IHoverTextBuilder<VariableSymbol>
{
    public string BuildHoverText( VariableSymbol symbol )
    {
        return symbol.Description.Value;
    }
}
