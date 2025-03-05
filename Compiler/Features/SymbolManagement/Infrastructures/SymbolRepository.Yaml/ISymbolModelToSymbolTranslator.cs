using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml;

public interface ISymbolModelToSymbolTranslator<in TSymbolModel, out TSymbol>
    : IDataTranslator<TSymbolModel, TSymbol>
    where TSymbol : SymbolBase;
