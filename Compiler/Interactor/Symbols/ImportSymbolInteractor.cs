using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class ImportSymbolInteractor<TSymbol> : IImportSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private readonly ISymbolImporter<TSymbol> importer;

    public ImportSymbolInteractor( ISymbolImporter<TSymbol> importer )
    {
        this.importer = importer;
    }

    public async Task<ImportSymbolOutputPort<TSymbol>> ExecuteAsync( UnitInputPort _, CancellationToken cancellationToken = default )
    {
        try
        {
            var result = await importer.ImportAsync( cancellationToken );
            return new ImportSymbolOutputPort<TSymbol>( true, result );
        }
        catch( Exception e )
        {
            return new ImportSymbolOutputPort<TSymbol>( false, new List<TSymbol>(), e );
        }
    }
}
