using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class ExportSymbolTemplateInputPort<TSymbol>(
    ISymbolExporter<TSymbol> inputInput
) : InputPort<ISymbolExporter<TSymbol>>( inputInput ) where TSymbol : SymbolBase;

public interface IExportSymbolTemplateUseCase<TSymbol>
    : IUseCase<ExportSymbolTemplateInputPort<TSymbol>, UnitOutputPort>
    where TSymbol : SymbolBase {}
