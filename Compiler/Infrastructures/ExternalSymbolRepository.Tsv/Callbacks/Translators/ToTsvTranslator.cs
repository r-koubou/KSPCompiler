using System.Collections.Generic;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
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
                  .AppendTab( v.Description )
                  .Append( v.AllowMultipleDeclarations.ToString().ToLower() );

            if( v.Arguments.Count == 0 )
            {
                result.AppendNewLine();

                continue;
            }

            result.AppendTab();

            foreach( var x in v.Arguments )
            {
                result.AppendTab( DataTypeUtility.ToString( x.DataType ), x.Name )
                      .AppendTab( x.RequiredDeclareOnInit.ToString().ToLower() )
                      .AppendTab( x.Description );
            }

            result.AppendNewLine();
        }

        return result.ToString();
    }
}
