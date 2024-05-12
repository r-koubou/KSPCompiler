using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Commands.Translators;
using KSPCompiler.UseCases.Symbols.Commons;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Commands;

public class TsvCommandSymbolImporter : IExternalCommandSymbolImporter
{
    private readonly ITextContentReader contentReader;

    public TsvCommandSymbolImporter( ITextContentReader reader )
    {
        contentReader = reader;
    }

    public async Task<ISymbolTable<CommandSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var tsv = await contentReader.ReadContentAsync( cancellationToken );

        return new FromTsvTranslator().Translate( tsv );
    }
}
