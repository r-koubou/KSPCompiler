using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.SignatureHelp;

public interface ISignatureHelpBuilder<in TSymbol> where TSymbol : SymbolBase
{
    SignatureHelpItem Build( TSymbol symbol, int activeParameter );
}
