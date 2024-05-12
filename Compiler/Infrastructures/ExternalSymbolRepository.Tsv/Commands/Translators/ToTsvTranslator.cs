using System.Collections.Generic;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Commands.Translators;

internal class ToTsvTranslator : IDataTranslator<IEnumerable<CommandSymbol>, string>
{
    public string Translate( IEnumerable<CommandSymbol> source )
    {
        var result = new StringBuilder();

        foreach( var v in source )
        {
            result.Append( v.Name ).Append( "\t" )
                  .Append( v.Reserved.ToString().ToLower() ).Append( "\t" )
                  .Append( v.Description ).Append( "\t" )
                  .Append( DataTypeUtility.ToString( v.DataType ) );

            if( v.Arguments.Count == 0 )
            {
                result.Append( "\n" );
                continue;
            }

            result.Append( "\t" );
            foreach( var x in v.Arguments )
            {
                result.Append( DataTypeUtility.ToString( x.DataType ) )
                      .Append( x.Name ).Append( "\t" )
                      .Append( x.Description ).Append( "\t" );
            }

            result.Append( "\n" );
        }

        return result.ToString();
    }
}
