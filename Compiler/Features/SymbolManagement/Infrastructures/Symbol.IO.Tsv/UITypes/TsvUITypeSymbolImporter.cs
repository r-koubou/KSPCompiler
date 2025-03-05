using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.UITypes.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.UITypes;

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

        return new TsvToSymbolTranslator().Translate( tsv );
    }
}
