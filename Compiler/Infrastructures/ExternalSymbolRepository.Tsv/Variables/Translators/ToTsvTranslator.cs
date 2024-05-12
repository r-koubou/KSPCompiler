using System.Collections.Generic;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;

internal class ToTsvTranslator : IDataTranslator<IEnumerable<VariableSymbol>, string>
{
    public string Translate( IEnumerable<VariableSymbol> source )
    {
        var result = new StringBuilder();

        foreach( var v in source )
        {
            result.Append( $"{v.Name}\t{v.Reserved.ToString().ToLower()}\t${v.Description}" )
                  .Append( '\n' );
        }

        return result.ToString();
    }
}
