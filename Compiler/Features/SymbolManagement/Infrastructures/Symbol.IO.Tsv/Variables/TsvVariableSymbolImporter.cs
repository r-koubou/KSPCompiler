using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.Gateways;
using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.Variables;

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

        return new TsvToSymbolTranslator().Translate( tsv );
    }
}
