using System.Collections.Generic;

using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.SignatureHelp;

public interface ISignatureHelpBuilder<in TSymbol> where TSymbol : SymbolBase
{
    SignatureHelpItem Build( TSymbol symbol, int activeParameter );
}

public interface IOverloadedSignatureHelpBuilder<in TSymbol> where TSymbol : SymbolBase
{
    SignatureHelpItem Build( IReadOnlyCollection<TSymbol> symbols, int activeSignature, int activeParameter );
}
