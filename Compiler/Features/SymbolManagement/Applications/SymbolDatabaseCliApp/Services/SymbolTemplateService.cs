using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.UseCase.ApplicationServices;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Local;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

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
