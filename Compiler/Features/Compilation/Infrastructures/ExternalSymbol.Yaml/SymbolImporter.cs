using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Gateways.Symbols;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml;

public abstract class SymbolImporter<TSymbol, TRootModel, TModel, TTranslator>( ITextContentReader reader )
    : ISymbolImporter<TSymbol>
    where TSymbol : SymbolBase
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
