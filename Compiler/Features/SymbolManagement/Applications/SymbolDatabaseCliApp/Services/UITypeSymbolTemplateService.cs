using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.UITypes;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

public sealed class UITypeSymbolTemplateService : SymbolTemplateService<UITypeSymbol>
{
    protected override ISymbolExporter<UITypeSymbol> GetExporter( ITextContentWriter writer )
        => new TsvUITypeSymbolExporter( writer );
}
