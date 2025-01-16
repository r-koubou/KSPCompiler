using System;

using KSPCompiler.Domain.Symbols;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.SignatureHelps;

public interface ISignatureHelpBuilder<in TSymbol> where TSymbol : SymbolBase
{
    SignatureHelp Build( TSymbol symbol );
}
