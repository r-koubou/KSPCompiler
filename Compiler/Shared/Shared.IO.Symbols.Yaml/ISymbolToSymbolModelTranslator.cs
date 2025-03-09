using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Shared.IO.Symbols.Yaml;

public interface ISymbolToSymbolModelTranslator<in TSymbol, out TSymbolModel>
    : IDataTranslator<TSymbol, TSymbolModel>
    where TSymbol : SymbolBase;
