using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.CoreNew.SignatureHelp;

public interface ISignatureHelpBuilder<in TSymbol> where TSymbol : SymbolBase
{
    SignatureHelpItem Build( TSymbol symbol, int activeParameter );
}
