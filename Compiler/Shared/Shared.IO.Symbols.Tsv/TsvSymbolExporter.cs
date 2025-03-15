using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CsvHelper;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.Shared.IO.Symbols.Tsv;

public class TsvSymbolExporter<TSymbol, TTranslator, THeaderRecordWriter>( ITextContentWriter contentWriter )
    : ISymbolExporter<TSymbol>
    where TSymbol : SymbolBase
    where TTranslator : IDataTranslator<IEnumerable<TSymbol>, string>, new()
    where THeaderRecordWriter : ITsvHeaderRecordWriter, new()
{
    private readonly ITextContentWriter contentWriter = contentWriter;

    public async Task ExportAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var sorted = symbols.OrderBy( x => x.Name.Value ).ToList();
        var translator = new TTranslator();
        var tsv = translator.Translate( sorted );

        await contentWriter.WriteContentAsync( tsv, cancellationToken );
    }

    public async Task ExportTemplateAsync( CancellationToken cancellationToken = default )
    {
        await using var writer = new StringWriter();
        await using var csvWriter = new CsvWriter( writer, ConstantValue.WriterConfiguration );
        var headerRecordWriter = new THeaderRecordWriter();

        headerRecordWriter.WriteHeaderRecord( csvWriter );
        await csvWriter.FlushAsync();

        await contentWriter.WriteContentAsync( writer.ToString(), cancellationToken );
    }

    public void Dispose() {}
}
