using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class VariableSymbolLoadInteractor : IVariableSymbolLoadUseCase
{
    private readonly ISymbolImporter<VariableSymbol> importer;

    public VariableSymbolLoadInteractor( ISymbolImporter<VariableSymbol> importer )
    {
        this.importer = importer;
    }

    public async Task<VariableSymbolLoadOutputData> ExecuteAsync( Unit input, CancellationToken cancellationToken = default )
    {
        try
        {
            var resultData = await importer.ImportAsync( cancellationToken );
            return new VariableSymbolLoadOutputData( true, resultData );
        }
        catch( Exception e )
        {
            return new VariableSymbolLoadOutputData( false, new List<VariableSymbol>(), e );
        }
    }
}
