using System;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public sealed class ImportSymbolInputPort<TSymbol>(
    ISymbolImporter<TSymbol> inputData
) : InputPort<ISymbolImporter<TSymbol>>( inputData ) where TSymbol : SymbolBase;

public sealed class ImportSymbolOutputPort(
    ImportSymbolOutputPortDetail outputData,
    bool result,
    Exception? error = null
) : OutputPort<ImportSymbolOutputPortDetail>( outputData, result, error );

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

public interface IImportSymbolUseCase<TSymbol>
    : IUseCase<ImportSymbolInputPort<TSymbol>, ImportSymbolOutputPort>
    where TSymbol : SymbolBase;
