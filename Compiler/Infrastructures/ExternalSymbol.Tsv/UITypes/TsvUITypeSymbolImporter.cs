using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.UITypes.Translators;

namespace KSPCompiler.ExternalSymbol.Tsv.UITypes;

public class TsvUITypeSymbolImporter : ISymbolImporter<UITypeSymbol>
{
    private readonly ITextContentReader contentReader;

    public TsvUITypeSymbolImporter( ITextContentReader reader )
    {
        contentReader = reader;
    }

    public async Task<IReadOnlyCollection<UITypeSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var tsv = await contentReader.ReadContentAsync( cancellationToken );

        return new FromTsvTranslator().Translate( tsv );
    }
}
