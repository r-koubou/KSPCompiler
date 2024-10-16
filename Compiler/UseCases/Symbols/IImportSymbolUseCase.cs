using System;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class ImportSymbolInputPort<TSymbol> : IInputPort<ISymbolImporter<TSymbol>> where TSymbol : SymbolBase
{
    public ISymbolImporter<TSymbol> InputData { get; }

    public ImportSymbolInputPort( ISymbolImporter<TSymbol> inputData )
    {
        InputData = inputData;
    }
}

public sealed class ImportSymbolOutputPort : IOutputPort<ImportSymbolOutputPortDetail>
{
    public bool Result { get; }
    public ImportSymbolOutputPortDetail OutputData { get; }
    public Exception? Error { get; }

    public ImportSymbolOutputPort( bool result, ImportSymbolOutputPortDetail outputData, Exception? error )
    {
        Result     = result;
        OutputData = outputData;
        Error      = error;
    }
}

public sealed class ImportSymbolOutputPortDetail
{
    public int CreatedCount { get; }
    public int UpdatedCount { get; }
    public int FailedCount  { get; }

    public ImportSymbolOutputPortDetail( int createdCount, int updatedCount, int failedCount )
    {
        CreatedCount = createdCount;
        UpdatedCount = updatedCount;
        FailedCount  = failedCount;
    }
}

public interface IImportSymbolUseCase<TSymbol> : IUseCase<ImportSymbolInputPort<TSymbol>, ImportSymbolOutputPort> where TSymbol : SymbolBase {}
