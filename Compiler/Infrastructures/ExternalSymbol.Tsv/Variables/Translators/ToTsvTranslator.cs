using System.Collections.Generic;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Extensions;

namespace KSPCompiler.ExternalSymbol.Tsv.Variables.Translators;

internal class ToTsvTranslator : IDataTranslator<IEnumerable<VariableSymbol>, string>
{
    public string Translate( IEnumerable<VariableSymbol> source )
    {
        var result = new StringBuilder();

        foreach( var v in source )
        {
            result.AppendTab( v.Name )
                  .AppendTab( v.BuiltIn.ToString().ToLower() )
                  .AppendTab( v.Description )
                  .AppendNewLine( v.BuiltIntoVersion );
        }

        return result.ToString();
    }
}
