using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables;

public class TsvVariableSymbolRepository : IVariableSymbolRepository
{
    private readonly ITextContentReader? contentReader;
    private readonly ITextContentWriter? contentWriter;

    public TsvVariableSymbolRepository( ITextContentWriter? writer ) : this( null, writer) {}

    public TsvVariableSymbolRepository( ITextContentReader? reader, ITextContentWriter? writer = null )
    {
        contentReader = reader;
        contentWriter = writer;
    }

    public async Task<ISymbolTable<VariableSymbol>> LoadAsync( CancellationToken cancellationToken = default )
    {
        if( contentReader == null )
        {
            throw new InvalidOperationException( "Content reader is not set." );
        }

        var tsv = await contentReader.ReadContentAsync( cancellationToken );

        return new FromTsvTranslator().Translate( tsv );
    }

    public async Task StoreAsync( ISymbolTable<VariableSymbol> store, CancellationToken cancellationToken = default )
    {
        if( contentWriter == null )
        {
            throw new InvalidOperationException( "Content writer is not set." );
        }

        var lines = new ToTsvTranslator().Translate( store );
        await contentWriter.WriteContentAsync( lines, cancellationToken );
    }

    public void Dispose() {}
}
