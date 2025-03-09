using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

public sealed class CallbackSymbolTemplateService : SymbolTemplateService<CallbackSymbol>
{
    protected override ISymbolExporter<CallbackSymbol> GetExporter( ITextContentWriter writer )
        => new TsvCallbackSymbolExporter( writer );
}
