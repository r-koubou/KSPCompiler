using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Variables;

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
