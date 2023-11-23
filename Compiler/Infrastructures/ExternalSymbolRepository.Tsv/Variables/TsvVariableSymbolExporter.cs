using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;
using KSPCompiler.UseCases.Symbols.Commons;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables;

public class TsvVariableSymbolExporter : IExternalVariableSymbolExporter
{
    private readonly ITextContentWriter contentWriter;

    public TsvVariableSymbolExporter( ITextContentWriter writer )
    {
        contentWriter = writer;
    }

    public async Task ExportAsync( ISymbolTable<VariableSymbol> store, CancellationToken cancellationToken = default )
    {
        var lines = new ToTsvTranslator().Translate( store );
        await contentWriter.WriteContentAsync( lines, cancellationToken );
    }

    public void Dispose() {}
}
