using System;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public sealed class ImportSymbolInputPort<TSymbol>(
    ISymbolImporter<TSymbol> inputInput
) : InputPort<ISymbolImporter<TSymbol>>( inputInput ) where TSymbol : SymbolBase;

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
