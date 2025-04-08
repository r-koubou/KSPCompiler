using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

public sealed class VariableSymbolTemplateService : SymbolTemplateService<VariableSymbol>
{
    protected override ISymbolExporter<VariableSymbol> GetExporter( ITextContentWriter writer )
        => new TsvVariableSymbolExporter( writer );
}
