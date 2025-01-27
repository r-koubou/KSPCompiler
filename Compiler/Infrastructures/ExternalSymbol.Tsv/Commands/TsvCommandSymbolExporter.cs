using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Commands.Translators;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.ExternalSymbol.Tsv.Commands;

public class TsvCommandSymbolExporter : ISymbolExporter<CommandSymbol>
{
    private readonly ITextContentWriter contentWriter;

    public TsvCommandSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( IEnumerable<CommandSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var tsv = new ToTsvTranslator().Translate( symbols );
        await contentWriter.WriteContentAsync( tsv, cancellationToken );
    }

    public void Dispose() {}
}
