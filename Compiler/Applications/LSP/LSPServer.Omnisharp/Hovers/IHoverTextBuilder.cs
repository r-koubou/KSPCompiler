using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.Omnisharp.Hovers;

public interface IHoverTextBuilder<in TSymbol> where TSymbol : SymbolBase
{
    string BuildHoverText( TSymbol symbol );
}
