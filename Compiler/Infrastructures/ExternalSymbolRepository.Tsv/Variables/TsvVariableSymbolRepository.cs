using System;
using System.IO;
using System.Text;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables;

public class TsvVariableSymbolRepository : IVariableSymbolRepository
{
    private readonly FilePath tsvFilePath;

    public TsvVariableSymbolRepository( FilePath tsvFilePath )
    {
        this.tsvFilePath = tsvFilePath;
    }

    public ISymbolTable<VariableSymbol> LoadSymbolTable()
    {
        var tsv = File.ReadAllLines( tsvFilePath.Path, Encoding.UTF8 );

        return new FromTsvTranslator().Translate( tsv );
    }

    public void StoreSymbolTable( ISymbolTable<VariableSymbol> store )
    {
        throw new NotSupportedException();
    }

    public void Dispose() {}
}
