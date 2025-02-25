using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class ExportSymbolTemplateInputPort<TSymbol>(
    ISymbolExporter<TSymbol> inputData
) : InputPort<ISymbolExporter<TSymbol>>( inputData ) where TSymbol : SymbolBase;

public interface IExportSymbolTemplateUseCase<TSymbol>
    : IUseCase<ExportSymbolTemplateInputPort<TSymbol>, UnitOutputPort>
    where TSymbol : SymbolBase {}
