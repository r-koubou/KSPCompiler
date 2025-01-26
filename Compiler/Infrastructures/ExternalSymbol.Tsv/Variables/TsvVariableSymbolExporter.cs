using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Variables.Translators;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.ExternalSymbol.Tsv.Variables;

public class TsvVariableSymbolExporter : ISymbolExporter<VariableSymbol>
{
    private readonly ITextContentWriter contentWriter;

    public TsvVariableSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( IEnumerable<VariableSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var tsv = new ToTsvTranslator().Translate( symbols );
        await contentWriter.WriteContentAsync( tsv, cancellationToken );
    }

    public void Dispose() {}
}
