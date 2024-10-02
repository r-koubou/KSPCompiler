using System;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class ExportSymbolInputData<TSymbol> : IInputPort<ExportSymbolInputDataDetail<TSymbol>> where TSymbol : SymbolBase
{
    public ExportSymbolInputDataDetail<TSymbol> InputData { get; }

    public ExportSymbolInputData( ExportSymbolInputDataDetail<TSymbol> inputData )
    {
        InputData = inputData;
    }
}

public sealed class ExportSymbolInputDataDetail<TSymbol> where TSymbol : SymbolBase
{
    public ISymbolExporter<TSymbol> Exporter { get; }
    public Predicate<TSymbol> Predicate { get; }

    public ExportSymbolInputDataDetail( ISymbolExporter<TSymbol> exporter, Predicate<TSymbol> predicate )
    {
        Exporter  = exporter;
        Predicate = predicate;
    }
}

public interface IExportSymbolUseCase<TSymbol> : IUseCase<ExportSymbolInputData<TSymbol>, UnitOutputPort> where TSymbol : SymbolBase {}
