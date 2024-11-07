using System.Collections.Generic;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Tsv.Extensions;

namespace KSPCompiler.ExternalSymbol.Tsv.Commands.Translators;

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
                  .AppendTab( v.BuiltIntoVersion )
                  .Append( DataTypeUtility.ToString( v.DataType ) );

            if( v.Arguments.Count == 0 )
            {
                result.AppendNewLine();
                continue;
            }

            result.AppendTab();

            var i = 0;

            foreach( var x in v.Arguments )
            {
                if( i == v.Arguments.Count - 1 )
                {
                    result.AppendTab( x.Name )
                          .Append( x.Description );
                }
                else
                {
                    result.AppendTab( x.Name )
                          .AppendTab( x.Description );
                }

                i++;
            }

            result.AppendNewLine();
        }

        return result.ToString();
    }
}
