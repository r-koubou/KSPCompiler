using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks;

public class TsvCallbackSymbolImporter : ISymbolImporter<CallbackSymbol>
{
    private readonly ITextContentReader contentReader;

    public TsvCallbackSymbolImporter( ITextContentReader reader )
    {
        contentReader = reader;
    }

    public async Task<IReadOnlyCollection<CallbackSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var tsv = await contentReader.ReadContentAsync( cancellationToken );

        return new TsvToSymbolTranslator().Translate( tsv );
    }
}
