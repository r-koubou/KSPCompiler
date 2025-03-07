using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CsvHelper;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables.Models.CsvHelperMappings;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables;

public class TsvVariableSymbolExporter : ISymbolExporter<VariableSymbol>
{
    private readonly ITextContentWriter contentWriter;

    public TsvVariableSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( IEnumerable<VariableSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var tsv = new SymbolToTsvTranslator().Translate( symbols );
        await contentWriter.WriteContentAsync( tsv, cancellationToken );
    }

    public async Task ExportTemplateAsync( CancellationToken cancellationToken = default )
    {
        await using var writer = new StringWriter();
        await using var csvWriter = new CsvWriter( writer, ConstantValue.WriterConfiguration );

        VariableColumnHeaderUtil.WriteHeader( csvWriter );
        await csvWriter.FlushAsync();

        await contentWriter.WriteContentAsync( writer.ToString(), cancellationToken );
    }

    public void Dispose() {}
}
