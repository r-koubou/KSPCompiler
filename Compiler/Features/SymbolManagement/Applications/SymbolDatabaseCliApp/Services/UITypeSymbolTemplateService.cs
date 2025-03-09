using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Symbols.Tsv.UITypes;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

public sealed class UITypeSymbolTemplateService : SymbolTemplateService<UITypeSymbol>
{
    protected override ISymbolExporter<UITypeSymbol> GetExporter( ITextContentWriter writer )
        => new TsvUITypeSymbolExporter( writer );
}
