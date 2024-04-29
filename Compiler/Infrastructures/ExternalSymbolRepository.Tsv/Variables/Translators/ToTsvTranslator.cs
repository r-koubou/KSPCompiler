using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;

internal class ToTsvTranslator : IDataTranslator<ISymbolTable<VariableSymbol>, string>
{
    public string Translate( ISymbolTable<VariableSymbol> source )
    {
        var result = new StringBuilder();

        foreach( var (_, v) in source.Table )
        {
            result.Append( $"{v.Name}\t{v.Reserved.ToString().ToLower()}\t${v.Description}" )
                  .Append( '\n' );
        }

        return result.ToString();
    }
}
