using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

public sealed class VariableSymbolTemplateService : SymbolTemplateService<VariableSymbol>
{
    protected override ISymbolExporter<VariableSymbol> GetExporter( ITextContentWriter writer )
        => new TsvVariableSymbolExporter( writer );
}
