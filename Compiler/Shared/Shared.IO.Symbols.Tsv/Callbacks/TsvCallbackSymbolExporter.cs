using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CsvHelper;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Models.CsvHelperMappings;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks;

public class TsvCallbackSymbolExporter : ISymbolExporter<CallbackSymbol>
{
    private readonly ITextContentWriter contentWriter;

    public TsvCallbackSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( IEnumerable<CallbackSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var tsv = new SymbolToTsvTranslator().Translate( symbols );
        await contentWriter.WriteContentAsync( tsv, cancellationToken );
    }

    public async Task ExportTemplateAsync( CancellationToken cancellationToken = default )
    {
        await using var writer = new StringWriter();
        await using var csvWriter = new CsvWriter( writer, ConstantValue.WriterConfiguration );

        ColumnHeaderUtil.WriteHeader( csvWriter );
        await csvWriter.FlushAsync();

        await contentWriter.WriteContentAsync( writer.ToString(), cancellationToken );
    }

    public void Dispose() {}
}
