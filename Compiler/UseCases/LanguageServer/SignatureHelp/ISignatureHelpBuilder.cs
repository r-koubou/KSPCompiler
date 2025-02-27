using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.LanguageServer.SignatureHelp;

public interface ISignatureHelpBuilder<in TSymbol> where TSymbol : SymbolBase
{
    SignatureHelpItem Build( TSymbol symbol, int activeParameter );
}
