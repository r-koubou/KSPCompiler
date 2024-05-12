using System.Text.RegularExpressions;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Commons;
using KSPCompiler.Infrastructures.Commons.Extensions;

using DataTypeUtility = KSPCompiler.Domain.Symbols.MetaData.DataTypeUtility;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Commands.Translators;

internal class FromTsvTranslator : IDataTranslator<string, ISymbolTable<CommandSymbol>>
{
    private enum Column
    {
        Name,
        Reserved,
        Description,
        ReturnType,
        ArgumentBegin,
        ArgumentDescription,
    }

    private static readonly Regex LineComment = new( @"^#.*" );

    public ISymbolTable<CommandSymbol> Translate( string source )
    {
        var result = new CommandSymbolTable();

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

            var command = new CommandSymbol
            {
                Name        = values[ (int)Column.Name ],
                Reserved    = values[ (int)Column.Reserved ].ToLower() == "true",
                Description = values[ (int)Column.Description ],
                DataType    = DataTypeUtility.Guess( values[ (int)Column.ReturnType ] )
            };

            if( values.Length > (int)Column.ArgumentBegin )
            {
                ParseArguments( values, ref command );
            }

            if( !result.Add( command ) )
            {
                throw new DuplicatedSymbolException( command.Name );
            }

        }

        return result;
    }

    private static void ParseArguments( string[] values, ref CommandSymbol command )
    {
        /*
         * Argument1, Argument1Description, Argument2, Argument2Description, ...
         */
        for( var i = (int)Column.ArgumentBegin; i < values.Length; i += 2 )
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

            command.AddArgument( argument );
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
