using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class ExportSymbolTemplateInputPort<TSymbol>( ISymbolExporter<TSymbol> inputData )
    : IInputPort<ISymbolExporter<TSymbol>>
    where TSymbol : SymbolBase
{
    public ISymbolExporter<TSymbol> InputData { get; } = inputData;
}

public interface IExportSymbolTemplateUseCase<TSymbol>
    : IUseCase<ExportSymbolTemplateInputPort<TSymbol>, UnitOutputPort>
    where TSymbol : SymbolBase {}
