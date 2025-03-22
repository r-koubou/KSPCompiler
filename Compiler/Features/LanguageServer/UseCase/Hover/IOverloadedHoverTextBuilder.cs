using System.Collections.Generic;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover;

public interface IOverloadedHoverTextBuilder<in TSymbol> where TSymbol : SymbolBase
{
    string BuildHoverText( IReadOnlyCollection<TSymbol> symbols );
}
