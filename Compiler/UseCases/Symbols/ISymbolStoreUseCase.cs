using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public interface ISymbolStoreUseCase<in TSymbol> : IUseCase<IEnumerable<TSymbol>, Unit> where TSymbol : SymbolBase
{}
