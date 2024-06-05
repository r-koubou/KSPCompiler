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
                  .Append( v.Description );

            if( v.InitializerArguments.Count == 0 )
            {
                result.AppendNewLine();

                continue;
            }

            result.AppendTab();

            foreach( var x in v.InitializerArguments )
            {
                result.AppendTab( x.Name )
                      .AppendTab( x.Description );
            }

            result.AppendNewLine();
        }

        return result.ToString();
    }
}
