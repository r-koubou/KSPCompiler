using System;
using System.IO;
using System.Text;

using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;
using KSPCompiler.Gateways;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables;

public class TsvExternalVariableSymbolRepository : IExternalSymbolRepository<VariableSymbol>
{
    private readonly FilePath tsvFilePath;

    public TsvExternalVariableSymbolRepository( FilePath tsvFilePath )
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
        throw new NotImplementedException();
    }
}
