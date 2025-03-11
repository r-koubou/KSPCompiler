using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover;

public interface IHoverTextBuilder<in TSymbol> where TSymbol : SymbolBase
{
    string BuildHoverText( TSymbol symbol );
}
