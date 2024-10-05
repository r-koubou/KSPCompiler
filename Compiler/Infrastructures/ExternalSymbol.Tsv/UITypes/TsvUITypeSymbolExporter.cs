using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.UITypes.Translators;

namespace KSPCompiler.ExternalSymbol.Tsv.UITypes;

public class TsvUITypeSymbolExporter : ISymbolExporter<UITypeSymbol>
{
    private readonly ITextContentWriter contentWriter;

    public TsvUITypeSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( IEnumerable<UITypeSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var tsv = new ToTsvTranslator().Translate( symbols );
        await contentWriter.WriteContentAsync( tsv, cancellationToken );
    }

    public void Dispose() {}
}
