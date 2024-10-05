using System.Collections.Generic;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Extensions;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;

internal class ToTsvTranslator : IDataTranslator<IEnumerable<VariableSymbol>, string>
{
    public string Translate( IEnumerable<VariableSymbol> source )
    {
        var result = new StringBuilder();

        foreach( var v in source )
        {
            result.AppendTab( v.Name )
                  .AppendTab( v.Reserved.ToString().ToLower() )
                  .AppendNewLine( v.Description );
        }

        return result.ToString();
    }
}
