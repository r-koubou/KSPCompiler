using System.Collections.Generic;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Extensions;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Callbacks.Translators;

internal class ToTsvTranslator : IDataTranslator<IEnumerable<CallbackSymbol>, string>
{
    public string Translate( IEnumerable<CallbackSymbol> source )
    {
        var result = new StringBuilder();

        foreach( var v in source )
        {
            result.AppendTab( v.Name )
                  .AppendTab( v.Reserved.ToString().ToLower() )
                  .AppendTab( v.AllowMultipleDeclaration.ToString().ToLower() )
                  .Append( v.Description );

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
                          .AppendTab( x.RequiredDeclareOnInit.ToString().ToLower() )
                          .Append( x.Description );
                }
                else
                {
                    result.AppendTab( x.Name )
                          .AppendTab( x.RequiredDeclareOnInit.ToString().ToLower() )
                          .AppendTab( x.Description );
                }

                i++;
            }

            result.AppendNewLine();
        }

        return result.ToString();
    }
}
