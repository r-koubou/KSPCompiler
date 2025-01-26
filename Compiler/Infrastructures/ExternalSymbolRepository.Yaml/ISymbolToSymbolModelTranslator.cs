using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.ExternalSymbolRepository.Yaml;

public interface ISymbolToSymbolModelTranslator<in TSymbol, out TSymbolModel>
    : IDataTranslator<TSymbol, TSymbolModel>
    where TSymbol : SymbolBase
    where TSymbolModel : ISymbolModel;
