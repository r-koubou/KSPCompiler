using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Callbacks.Translators;

namespace KSPCompiler.ExternalSymbol.Tsv.Callbacks;

public class TsvCallbackSymbolExporter : ISymbolExporter<CallbackSymbol>
{
    private readonly ITextContentWriter contentWriter;

    public TsvCallbackSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( IEnumerable<CallbackSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var tsv = new ToTsvTranslator().Translate( symbols );
        await contentWriter.WriteContentAsync( tsv, cancellationToken );
    }

    public void Dispose() {}
}
