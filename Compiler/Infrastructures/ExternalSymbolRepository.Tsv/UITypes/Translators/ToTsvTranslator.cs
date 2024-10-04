using System.Collections.Generic;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Tsv.Extensions;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.UITypes.Translators;

internal class ToTsvTranslator : IDataTranslator<IEnumerable<UITypeSymbol>, string>
{
    public string Translate( IEnumerable<UITypeSymbol> source )
    {
        var result = new StringBuilder();

        foreach( var v in source )
        {
            result.AppendTab( v.Name )
                  .AppendTab( v.Reserved.ToString().ToLower() )
                  .AppendTab( DataTypeUtility.ToString( v.DataType ) )
                  .AppendTab( v.Description )
                  .Append( v.InitializerRequired.ToString().ToLower() );

            if( v.InitializerArguments.Count == 0 )
            {
                result.AppendNewLine();

                continue;
            }

            result.AppendTab();

            var i = 0;

            foreach( var x in v.InitializerArguments )
            {
                if( i == v.InitializerArguments.Count - 1 )
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
