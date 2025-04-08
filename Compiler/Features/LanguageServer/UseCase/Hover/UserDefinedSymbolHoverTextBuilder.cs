using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover;

public class UserDefinedSymbolHoverTextBuilder<TSymbol> : IHoverTextBuilder<TSymbol> where TSymbol : SymbolBase
{
    public string BuildHoverText( TSymbol symbol )
    {
        return DocumentUtility.GetCommentOrDescriptionText( symbol );
    }
}
