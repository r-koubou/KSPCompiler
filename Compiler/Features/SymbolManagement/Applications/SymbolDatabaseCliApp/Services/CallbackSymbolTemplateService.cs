using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Callbacks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;

namespace KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;

public sealed class CallbackSymbolTemplateService : SymbolTemplateService<CallbackSymbol>
{
    protected override ISymbolExporter<CallbackSymbol> GetExporter( ITextContentWriter writer )
        => new TsvCallbackSymbolExporter( writer );
}
