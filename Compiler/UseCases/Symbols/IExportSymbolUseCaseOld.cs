using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public class ExportSymbolInputDataOld<TSymbol> : IInputPort<IEnumerable<TSymbol>> where TSymbol : SymbolBase
{
    public IEnumerable<TSymbol> InputData { get; }

    public ExportSymbolInputDataOld( IEnumerable<TSymbol> inputData )
    {
        InputData = inputData;
    }
}

public interface IExportSymbolUseCaseOld<TSymbol> : IUseCase<ExportSymbolInputDataOld<TSymbol>, UnitOutputPort> where TSymbol : SymbolBase {}
