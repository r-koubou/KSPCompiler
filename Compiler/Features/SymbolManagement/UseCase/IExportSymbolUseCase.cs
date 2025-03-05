using System;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public sealed class ExportSymbolInputData<TSymbol>(
    ExportSymbolInputDataDetail<TSymbol> inputInput
) : InputPort<ExportSymbolInputDataDetail<TSymbol>>( inputInput ) where TSymbol : SymbolBase;

public sealed class ExportSymbolInputDataDetail<TSymbol>(
    ISymbolExporter<TSymbol> exporter,
    Predicate<TSymbol> predicate
) where TSymbol : SymbolBase
{
    public ISymbolExporter<TSymbol> Exporter { get; } = exporter;
    public Predicate<TSymbol> Predicate { get; } = predicate;
}

public interface IExportSymbolUseCase<TSymbol>
    : IUseCase<ExportSymbolInputData<TSymbol>, UnitOutputPort>
    where TSymbol : SymbolBase {}
