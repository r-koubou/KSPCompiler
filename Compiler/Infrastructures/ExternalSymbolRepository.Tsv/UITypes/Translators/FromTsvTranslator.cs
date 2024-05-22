using System.Collections.Generic;
using System.Text.RegularExpressions;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Infrastructures.Commons.Extensions;

using DataTypeUtility = KSPCompiler.Domain.Symbols.MetaData.DataTypeUtility;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.UITypes.Translators;

internal class FromTsvTranslator : IDataTranslator<string, IReadOnlyCollection<UITypeSymbol>>
{
    private enum Column
    {
        Name,
        VariableType,
        Description,
        RequireInitializer,
        InitializerArguments,
    }

    private static readonly Regex LineComment = new( @"^#.*" );

    public IReadOnlyCollection<UITypeSymbol> Translate( string source )
    {
        var result = new List<UITypeSymbol>();

        foreach( var x in source.SplitNewLine() )
        {
            if( string.IsNullOrWhiteSpace( x ) )
            {
                continue;
            }

            if( LineComment.IsMatch( x ) )
            {
                continue;
            }

            var values = x.Split( '\t' );

            // Remove " from the beginning and end of the string.
            RemoveQuoteCharacter( values );

            var uiType = new UITypeSymbol( values[ (int)Column.RequireInitializer ].ToLower() == "true" )
            {
                Name        = values[ (int)Column.Name ],
                Reserved    = true,
                Description = values[ (int)Column.Description ],
                DataType    = DataTypeUtility.Guess( values[ (int)Column.VariableType ] )
            };

            ParseInitializerArguments( values, ref uiType );
            result.Add( uiType );
        }

        return result;
    }

    private static void ParseInitializerArguments( string[] values, ref UITypeSymbol uiType )
    {
        /*
         * Argument1, Argument1Description, Argument2, Argument2Description, ...
         */
        for( var i = (int)Column.InitializerArguments; i < values.Length; i += 2 )
        {
            var name = values[ i + 0 ];
            var description = values[ i + 1 ];

            var argument = new VariableSymbol
            {
                Name        = name,
                Reserved    = false,
                Description = description,
            };

            argument.DataType = DataTypeUtility.Guess( argument.Name );

            uiType.AddInitializerArgument( argument );
        }
    }

    private static void RemoveQuoteCharacter( string[] values )
    {
        for( var i = 0; i < values.Length; i++ )
        {
            var v = values[ i ];

            if( v.StartsWith( "\"" ) && v.EndsWith( "\"" ) )
            {
                values[ i ] = v[ 1..^1 ];
            }
        }
    }
}
