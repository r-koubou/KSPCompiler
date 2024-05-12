using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Commands.Translators;
using KSPCompiler.UseCases.Symbols.Commons;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Commands;

public class TsvCommandSymbolExporter : IExternalCommandSymbolExporter
{
    private readonly ITextContentWriter contentWriter;

    public TsvCommandSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( ISymbolTable<CommandSymbol> store, CancellationToken cancellationToken = default )
    {
        var tsv = new ToTsvTranslator().Translate( store );
        await contentWriter.WriteContentAsync( tsv, cancellationToken );
    }

    public void Dispose() {}
}
