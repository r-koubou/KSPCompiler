using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;

public sealed class ExportSymbolTemplateInputPort<TSymbol>(
    ISymbolExporter<TSymbol> inputInput
) : InputPort<ISymbolExporter<TSymbol>>( inputInput ) where TSymbol : SymbolBase;

public interface IExportSymbolTemplateUseCase<TSymbol>
    : IUseCase<ExportSymbolTemplateInputPort<TSymbol>, UnitOutputPort>
    where TSymbol : SymbolBase {}
