using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

[Obsolete]
public class ImportSymbolInteractorOld<TSymbol> : IImportSymbolUseCaseOld<TSymbol> where TSymbol : SymbolBase
{
    private readonly ISymbolImporter<TSymbol> importer;

    public ImportSymbolInteractorOld( ISymbolImporter<TSymbol> importer )
    {
        this.importer = importer;
    }

    public async Task<ImportSymbolOutputPortOld<TSymbol>> ExecuteAsync( UnitInputPort _, CancellationToken cancellationToken = default )
    {
        try
        {
            var result = await importer.ImportAsync( cancellationToken );
            return new ImportSymbolOutputPortOld<TSymbol>( true, result );
        }
        catch( Exception e )
        {
            return new ImportSymbolOutputPortOld<TSymbol>( false, new List<TSymbol>(), e );
        }
    }
}
