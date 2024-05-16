using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class ImportSymbolInteractor<TSymbol> : IImportSymbolUseCase<TSymbol> where TSymbol : SymbolBase
{
    private readonly ISymbolImporter<TSymbol> importer;

    public ImportSymbolInteractor( ISymbolImporter<TSymbol> importer )
    {
        this.importer = importer;
    }

    public async Task<SymbolLoadOutputData<TSymbol>> ExecuteAsync( Unit _, CancellationToken cancellationToken = default )
    {
        try
        {
            var resultData = await importer.ImportAsync( cancellationToken );
            return new SymbolLoadOutputData<TSymbol>( true, resultData );
        }
        catch( Exception e )
        {
            return new SymbolLoadOutputData<TSymbol>( false, new List<TSymbol>(), e );
        }
    }
}
