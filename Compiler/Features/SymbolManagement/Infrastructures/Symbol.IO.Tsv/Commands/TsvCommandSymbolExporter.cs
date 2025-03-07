using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CsvHelper;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands.Models.CsvHelperMappings;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Commands;

public class TsvCommandSymbolExporter : ISymbolExporter<CommandSymbol>
{
    private readonly ITextContentWriter contentWriter;

    public TsvCommandSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( IEnumerable<CommandSymbol> symbols, CancellationToken cancellationToken = default )
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

        await contentWriter.WriteContentAsync( writer.ToString(), cancellationToken );    }

    public void Dispose() {}
}
