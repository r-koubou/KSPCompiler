using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CsvHelper;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.UITypes.Models.CsvHelperMappings;
using KSPCompiler.ExternalSymbol.Tsv.UITypes.Translators;
using KSPCompiler.Gateways.Symbols;

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
        var tsv = new SymbolToTsvTranslator().Translate( symbols );
        await contentWriter.WriteContentAsync( tsv, cancellationToken );
    }

    public async Task ExportTemplateAsync( CancellationToken cancellationToken = default )
    {
        await using var writer = new StringWriter();
        await using var csvWriter = new CsvWriter( writer, ConstantValue.WriterConfiguration );

        UITypeColumnHeaderUtil.WriteHeader( csvWriter );
        await csvWriter.FlushAsync();

        await contentWriter.WriteContentAsync( writer.ToString(), cancellationToken );
    }

    public void Dispose() {}
}
