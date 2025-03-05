using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml;

public interface ISymbolModelToSymbolTranslator<in TSymbolModel, out TSymbol>
    : IDataTranslator<TSymbolModel, TSymbol>
    where TSymbolModel : ISymbolModel
    where TSymbol : SymbolBase;
