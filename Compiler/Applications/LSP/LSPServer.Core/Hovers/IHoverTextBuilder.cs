using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.Core.Hovers;

public interface IHoverTextBuilder<in TSymbol> where TSymbol : SymbolBase
{
    string BuildHoverText( TSymbol symbol );
}
