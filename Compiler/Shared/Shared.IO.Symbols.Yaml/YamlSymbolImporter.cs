using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.Shared.IO.Symbols.Yaml;

public abstract class YamlSymbolImporter<TSymbol, TRootModel, TModel, TTranslator>( ITextContentReader reader )
    : ISymbolImporter<TSymbol>
    where TSymbol : SymbolBase
    where TModel : ISymbolModel
    where TRootModel : ISymbolRootModel<TModel>, new()
    where TTranslator : ISymbolModelToSymbolTranslator<TModel, TSymbol>, new()
{
    private readonly ITextContentReader contentReader = reader;

    public async Task<IReadOnlyCollection<TSymbol>> ImportAsync( CancellationToken cancellationToken = default )
    {
        var yaml = await contentReader.ReadContentAsync( cancellationToken );
        var root = ConstantValue.YamlDeserializer.Deserialize<TRootModel>( yaml );

        var symbols = new List<TSymbol>();
        var translator = new TTranslator();

        foreach( var x in root.Data )
        {
            symbols.Add( translator.Translate( x ) );
        }

        return symbols;
    }
}
