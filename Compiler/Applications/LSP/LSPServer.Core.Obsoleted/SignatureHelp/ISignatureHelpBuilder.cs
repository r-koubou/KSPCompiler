using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.Core.SignatureHelp;

public interface ISignatureHelpBuilder<in TSymbol> where TSymbol : SymbolBase
{
    SignatureHelpItem Build( TSymbol symbol, int activeParameter );
}
