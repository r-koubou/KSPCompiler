using System.Collections.Generic;
using System.Linq;
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

            TranslateArgument( result, v );

            result.AppendNewLine();
        }

        return result.ToString();
    }

    private static void TranslateArgument( StringBuilder result, CommandSymbol v )
    {
        var i = 0;

        foreach( var x in v.Arguments )
        {
            TranslateArgumentType( result, x );

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
    }

    private static void TranslateArgumentType( StringBuilder result, CommandArgumentSymbol x )
    {
        result.Append( DataTypeUtility.ToString( x.DataType ) );

        if( x.UITypeNames.Any() )
        {
            result.Append( "||" );
            TranslateArgumentType( result, x.UITypeNames );
        }

        if( x.OtherTypeNames.Any() )
        {
            result.Append( "||" );
            TranslateArgumentType( result, x.OtherTypeNames );
        }

        result.AppendTab();
    }

    private static void TranslateArgumentType( StringBuilder result, IReadOnlyList<string> types )
    {
        var count = types.Count;

        for( var k = 0; k < count; k++ )
        {
            result.Append( types[ k ] );

            if( k != count - 1 )
            {
                result.Append( "||" );
            }
        }
    }
}
