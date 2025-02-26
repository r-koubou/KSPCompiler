using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Interactors.LanguageServer.Hover;

public interface IHoverTextBuilder<in TSymbol> where TSymbol : SymbolBase
{
    string BuildHoverText( TSymbol symbol );
}
