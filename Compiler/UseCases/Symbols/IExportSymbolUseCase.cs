using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public class ExportSymbolInputData<TSymbol> : IInputPort<IEnumerable<TSymbol>> where TSymbol : SymbolBase
{
    public IEnumerable<TSymbol> InputData { get; }

    public ExportSymbolInputData( IEnumerable<TSymbol> inputData )
    {
        InputData = inputData;
    }
}

public interface IExportSymbolUseCase<TSymbol> : IUseCase<ExportSymbolInputData<TSymbol>, UnitOutputPort> where TSymbol : SymbolBase {}
