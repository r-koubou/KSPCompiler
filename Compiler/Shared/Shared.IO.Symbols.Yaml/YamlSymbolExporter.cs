using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Abstractions.Symbol;

namespace KSPCompiler.Shared.IO.Symbols.Yaml;

public abstract class YamlSymbolExporter<TSymbol, TRootModel, TModel, TTranslator>( ITextContentWriter writer )
    : ISymbolExporter<TSymbol>
    where TSymbol : SymbolBase
    where TModel : ISymbolModel
    where TRootModel : ISymbolRootModel<TModel>, new()
    where TTranslator : ISymbolToSymbolModelTranslator<TSymbol, TModel>, new()
{
    private readonly ITextContentWriter contentWriter = writer;

    public async Task ExportAsync( IEnumerable<TSymbol> symbols, CancellationToken cancellationToken = default )
    {
        var sorted = symbols.OrderBy( x => x.Name.Value ).ToList();
        var models = new List<TModel>();
        var translator = new TTranslator();

        foreach( var symbol in sorted )
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
