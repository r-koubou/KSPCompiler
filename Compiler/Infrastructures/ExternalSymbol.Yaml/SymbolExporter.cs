using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.ExternalSymbol.Yaml;

public abstract class SymbolExporter<TSymbol, TRootModel, TModel, TTranslator>( ITextContentWriter writer )
    : ISymbolExporter<TSymbol>
    where TSymbol : SymbolBase
    where TRootModel : ISymbolRootModel<TModel>, new()
    where TTranslator : ISymbolToSymbolModelTranslator<TSymbol, TModel>, new()
{
    private readonly ITextContentWriter contentWriter = writer;

    public async Task ExportAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var models = new List<TModel>();
        var translator = new TTranslator();

        foreach( var symbol in symbols )
        {
            models.Add( translator.Translate( symbol ) );
        }

        var root = new TRootModel
        {
            Data = models
        };

        var yaml = ConstantValue.YamlSerializer.Serialize( root );

        await contentWriter.WriteContentAsync( yaml, cancellationToken );
    }

    public void Dispose() {}
}
