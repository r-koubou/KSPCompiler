using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Variables;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.Applications.SymbolDbManager.Services;

public sealed class VariableSymbolTemplateService : SymbolTemplateService<VariableSymbol>
{
    protected override ISymbolExporter<VariableSymbol> GetExporter( ITextContentWriter writer )
        => new TsvVariableSymbolExporter( writer );
}
