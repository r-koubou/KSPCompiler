using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;
using KSPCompiler.Shared.IO.Symbols.Tsv.Commands.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Commands;

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

        return new TsvToSymbolTranslator().Translate( tsv );
    }
}
