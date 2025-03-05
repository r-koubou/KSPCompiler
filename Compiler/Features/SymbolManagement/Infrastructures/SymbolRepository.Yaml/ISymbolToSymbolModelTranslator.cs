using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml;

public interface ISymbolToSymbolModelTranslator<in TSymbol, out TSymbolModel>
    : IDataTranslator<TSymbol, TSymbolModel>
    where TSymbol : SymbolBase;
