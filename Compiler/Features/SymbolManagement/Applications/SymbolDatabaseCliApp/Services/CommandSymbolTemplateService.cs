using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

public sealed class CommandSymbolTemplateService : SymbolTemplateService<CommandSymbol>
{
    protected override ISymbolExporter<CommandSymbol> GetExporter( ITextContentWriter writer )
        => new TsvCommandSymbolExporter( writer );
}
