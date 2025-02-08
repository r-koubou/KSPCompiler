using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Commands;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.Applications.SymbolDbManager.Services;

public sealed class CommandSymbolTemplateService : SymbolTemplateService<CommandSymbol>
{
    protected override ISymbolExporter<CommandSymbol> GetExporter( ITextContentWriter writer )
        => new TsvCommandSymbolExporter( writer );
}
