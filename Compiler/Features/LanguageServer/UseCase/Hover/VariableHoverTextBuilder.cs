using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover;

public class VariableHoverTextBuilder : IHoverTextBuilder<VariableSymbol>
{
    public string BuildHoverText( VariableSymbol symbol )
    {
        return symbol.Description.Value;
    }
}
