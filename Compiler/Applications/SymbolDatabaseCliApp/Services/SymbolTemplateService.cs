using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.Infrastructures.Commons.LocalStorages;
using KSPCompiler.Interactors.ApplicationServices.Symbols;

namespace KSPCompiler.Applications.SymbolDbManager.Services;

public abstract class SymbolTemplateService<TSymbol> : ISymbolTemplateService where TSymbol : SymbolBase
{
    protected abstract ISymbolExporter<TSymbol> GetExporter( ITextContentWriter writer );

    public async Task<ExportResult> ExportSymbolTemplateAsync( string exportFilePath, CancellationToken cancellationToken = default )
    {
        try
        {
            var exportPath = new FilePath( exportFilePath );
            var writer = new LocalTextContentWriter( exportPath );
            var exporter = GetExporter( writer );
            var applicationService = new SymbolTemplateApplicationService<TSymbol>();

            return await applicationService.ExportAsync( exporter, cancellationToken );
        }
        catch( Exception e )
        {
            return new ExportResult( false, e );
        }
    }
}
