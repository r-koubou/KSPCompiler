using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Models;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Translators;

public class ToCommandSymbolModelTranslator : IDataTranslator<CommandSymbol, CommandSymbolModel>
{
    public CommandSymbolModel Translate( CommandSymbol source )
    {
        var model = new CommandSymbolModel
        {
            Id               = Guid.NewGuid(),
            Name             = source.Name.Value,
            BuiltIn          = source.BuiltIn,
            ReturnType       = DataTypeUtility.ToString( source.DataType ),
            Description      = source.Description.Value,
            BuiltIntoVersion = source.BuiltIntoVersion
        };

        foreach( var arg in source.Arguments )
        {
            var argTypeStringBuilder = new StringBuilder();

            TranslateArgumentType( argTypeStringBuilder, arg );

            var argument = new CommandArgumentModel
            {
                DataType    = argTypeStringBuilder.ToString(),
                Name        = arg.Name,
                Description = arg.Description
            };

            model.Arguments.Add( argument );
        }

        return model;
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
