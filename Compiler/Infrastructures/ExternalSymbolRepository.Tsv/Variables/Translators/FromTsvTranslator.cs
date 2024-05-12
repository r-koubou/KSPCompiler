using System.Collections.Generic;
using System.Text.RegularExpressions;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Infrastructures.Commons.Extensions;

using DataTypeUtility = KSPCompiler.Domain.Symbols.MetaData.DataTypeUtility;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;

internal class FromTsvTranslator : IDataTranslator<string, IReadOnlyCollection<VariableSymbol>>
{
    private enum Column
    {
        Name,
        Reserved,
        Description
    }

    private static readonly Regex LineComment = new( @"^#.*" );

    public IReadOnlyCollection<VariableSymbol> Translate( string source )
    {
        var result = new List<VariableSymbol>();

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

            var symbol = new VariableSymbol
            {
                Name        = values[ (int)Column.Name ],
                ArraySize   = 0,
                Reserved    = values[ (int)Column.Reserved ].ToLower() == "true",
                Description = values[ (int)Column.Description ]
            };

            symbol.DataType         = DataTypeUtility.Guess( symbol.Name );
            symbol.DataTypeModifier = DataTypeModifierFlag.Const;

            result.Add( symbol );
        }

        return result;
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
