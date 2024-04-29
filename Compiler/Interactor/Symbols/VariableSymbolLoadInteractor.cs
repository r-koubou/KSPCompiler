using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;
using KSPCompiler.UseCases.Symbols.Commons;

namespace KSPCompiler.Interactor.Symbols;

public class VariableSymbolLoadInteractor : IVariableSymbolLoadUseCase
{
    private readonly IExternalVariableSymbolImporter importer;

    public VariableSymbolLoadInteractor( IExternalVariableSymbolImporter importer )
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
            return new VariableSymbolLoadOutputData( false, new VariableSymbolTable(), e );
        }
    }
}
