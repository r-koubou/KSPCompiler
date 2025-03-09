using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Shared.IO.Symbols.Yaml;

public interface ISymbolModelToSymbolTranslator<in TSymbolModel, out TSymbol>
    : IDataTranslator<TSymbolModel, TSymbol>
    where TSymbolModel : ISymbolModel
    where TSymbol : SymbolBase;
