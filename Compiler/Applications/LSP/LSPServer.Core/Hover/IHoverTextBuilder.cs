using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.Core.Hover;

public interface IHoverTextBuilder<in TSymbol> where TSymbol : SymbolBase
{
    string BuildHoverText( TSymbol symbol );
}
