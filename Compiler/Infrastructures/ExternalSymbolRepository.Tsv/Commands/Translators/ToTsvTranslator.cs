using System.Collections.Generic;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Tsv.Extensions;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Commands.Translators;

internal class ToTsvTranslator : IDataTranslator<IEnumerable<CommandSymbol>, string>
{
    public string Translate( IEnumerable<CommandSymbol> source )
    {
        var result = new StringBuilder();

        foreach( var v in source )
        {
            result.AppendTab( v.Name )
                  .AppendTab( v.Reserved.ToString().ToLower() )
                  .AppendTab( v.Description )
                  .Append( DataTypeUtility.ToString( v.DataType ) );

            if( v.Arguments.Count == 0 )
            {
                result.AppendNewLine();
                continue;
            }

            result.AppendTab();
            foreach( var x in v.Arguments )
            {
                result.AppendTab( x.Name )
                      .AppendTab( x.Description );
            }

            result.AppendNewLine();
        }

        return result.ToString();
    }
}
