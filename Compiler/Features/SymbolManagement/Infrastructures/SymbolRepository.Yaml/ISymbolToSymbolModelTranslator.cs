using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.SymbolManagement.Repository.Yaml;

public interface ISymbolToSymbolModelTranslator<in TSymbol, out TSymbolModel>
    : IDataTranslator<TSymbol, TSymbolModel>
    where TSymbol : SymbolBase
    where TSymbolModel : ISymbolModel;
