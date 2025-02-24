using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.Core.Hover;

public class VariableHoverTextBuilder : IHoverTextBuilder<VariableSymbol>
{
    public string BuildHoverText( VariableSymbol symbol )
    {
        return symbol.Description.Value;
    }
}
