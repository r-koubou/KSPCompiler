using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Symbols.Tsv.Commands;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

public sealed class CommandSymbolTemplateService : SymbolTemplateService<CommandSymbol>
{
    protected override ISymbolExporter<CommandSymbol> GetExporter( ITextContentWriter writer )
        => new TsvCommandSymbolExporter( writer );
}
