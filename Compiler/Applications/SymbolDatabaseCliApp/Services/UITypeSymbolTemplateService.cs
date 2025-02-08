using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.UITypes;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.Applications.SymbolDbManager.Services;

public sealed class UITypeSymbolTemplateService : SymbolTemplateService<UITypeSymbol>
{
    protected override ISymbolExporter<UITypeSymbol> GetExporter( ITextContentWriter writer )
        => new TsvUITypeSymbolExporter( writer );
}
