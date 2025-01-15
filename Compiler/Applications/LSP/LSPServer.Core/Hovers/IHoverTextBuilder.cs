using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.LSPServer.Core.Hovers;

public interface IHoverTextBuilder<in TSymbol> where TSymbol : SymbolBase
{
    string BuildHoverText( TSymbol symbol );
}
