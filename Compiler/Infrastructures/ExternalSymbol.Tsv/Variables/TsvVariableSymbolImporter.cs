using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Variables.Translators;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.ExternalSymbol.Tsv.Variables;

public class TsvVariableSymbolImporter : ISymbolImporter<VariableSymbol>
{
    private readonly ITextContentReader contentReader;

    public TsvVariableSymbolImporter( ITextContentReader reader )
    {
        contentReader = reader;
    }

    public async Task<IReadOnlyCollection<VariableSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var tsv = await contentReader.ReadContentAsync( cancellationToken );

        return new FromTsvTranslator().Translate( tsv );
    }
}
