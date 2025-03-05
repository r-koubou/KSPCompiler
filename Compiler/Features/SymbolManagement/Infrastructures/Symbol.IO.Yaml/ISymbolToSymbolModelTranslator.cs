using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml;

public interface ISymbolToSymbolModelTranslator<in TSymbol, out TSymbolModel>
    : IDataTranslator<TSymbol, TSymbolModel>
    where TSymbol : SymbolBase;
