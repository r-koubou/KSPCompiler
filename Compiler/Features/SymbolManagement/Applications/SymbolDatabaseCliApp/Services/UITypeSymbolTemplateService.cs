using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.UITypes;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

public sealed class UITypeSymbolTemplateService : SymbolTemplateService<UITypeSymbol>
{
    protected override ISymbolExporter<UITypeSymbol> GetExporter( ITextContentWriter writer )
        => new TsvUITypeSymbolExporter( writer );
}
