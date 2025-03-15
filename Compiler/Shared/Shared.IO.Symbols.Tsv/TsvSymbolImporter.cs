using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.Shared.IO.Symbols.Tsv;

public abstract class TsvSymbolImporter<TSymbol, TTranslator>( ITextContentReader reader )
    : ISymbolImporter<TSymbol>
    where TSymbol : SymbolBase
    where TTranslator : IDataTranslator<string, IReadOnlyCollection<TSymbol>>, new()
{
    private readonly ITextContentReader contentReader = reader;

    public async Task<IReadOnlyCollection<TSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var tsv = await contentReader.ReadContentAsync( cancellationToken );
        var translator = new TTranslator();

        return translator.Translate( tsv );
    }
}
