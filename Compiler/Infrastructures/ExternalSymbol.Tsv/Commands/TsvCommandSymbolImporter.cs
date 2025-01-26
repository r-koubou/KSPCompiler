using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Commands.Translators;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.ExternalSymbol.Tsv.Commands;

public class TsvCommandSymbolImporter : ISymbolImporter<CommandSymbol>
{
    private readonly ITextContentReader contentReader;

    public TsvCommandSymbolImporter( ITextContentReader reader )
    {
        contentReader = reader;
    }

    public async Task<IReadOnlyCollection<CommandSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var tsv = await contentReader.ReadContentAsync( cancellationToken );

        return new FromTsvTranslator().Translate( tsv );
    }
}
