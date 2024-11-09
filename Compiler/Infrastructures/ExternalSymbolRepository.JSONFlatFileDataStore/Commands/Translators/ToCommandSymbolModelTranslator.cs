using System.Collections.Generic;
using System.Linq;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Translators;

internal class ToCommandSymbolModelTranslator : IDataTranslator<IEnumerable<CommandSymbol>, IReadOnlyCollection<CommandSymbolModel>>
{
    public IReadOnlyCollection<CommandSymbolModel> Translate( IEnumerable<CommandSymbol> source )
    {
        var result = new List<CommandSymbolModel>();

        foreach( var x in source )
        {
            var symbol = new CommandSymbolModel
            {
                Name             = x.Name.Value,
                Reserved         = x.Reserved,
                ReturnType       = DataTypeUtility.ToString( x.DataType ),
                Description      = x.Description.Value,
                BuiltIntoVersion = x.BuiltIntoVersion
            };

            foreach( var arg in x.Arguments )
            {
                var argTypeStringBuilder = new StringBuilder();

                TranslateArgumentType( argTypeStringBuilder, arg );

                var argument = new CommandArgumentModel
                {
                    DataType    = argTypeStringBuilder.ToString(),
                    Name        = arg.Name,
                    Description = arg.Description
                };

                symbol.Arguments.Add( argument );
            }

            result.Add( symbol );
        }

        return result;
    }

    private static void TranslateArgumentType( StringBuilder result, CommandArgumentSymbol x )
    {
        var appendSeparator = false;

        if( x.DataType != DataTypeFlag.None )
        {
            appendSeparator = true;
            result.Append( DataTypeUtility.ToString( x.DataType ) );
        }

        if( x.UITypeNames.Any() )
        {
            if( appendSeparator )
            {
                result.Append( "||" );
            }
            appendSeparator = x.UITypeNames.Any();
            TranslateArgumentType( result, x.UITypeNames );
        }

        if( x.OtherTypeNames.Any() )
        {
            if( appendSeparator )
            {
                result.Append( "||" );
            }
            TranslateArgumentType( result, x.OtherTypeNames );
        }
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
