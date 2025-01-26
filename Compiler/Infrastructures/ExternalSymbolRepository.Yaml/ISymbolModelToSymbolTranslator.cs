using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.ExternalSymbolRepository.Yaml;

public interface ISymbolModelToSymbolTranslator<in TSymbolModel, out TSymbol>
    : IDataTranslator<TSymbolModel, TSymbol>
    where TSymbolModel : ISymbolModel
    where TSymbol : SymbolBase;
