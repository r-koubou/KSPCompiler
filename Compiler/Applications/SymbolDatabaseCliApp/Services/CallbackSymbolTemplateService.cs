using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Callbacks;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.Apps.SymbolDbManager.Services;

public sealed class CallbackSymbolTemplateService : SymbolTemplateService<CallbackSymbol>
{
    protected override ISymbolExporter<CallbackSymbol> GetExporter( ITextContentWriter writer )
        => new TsvCallbackSymbolExporter( writer );
}
