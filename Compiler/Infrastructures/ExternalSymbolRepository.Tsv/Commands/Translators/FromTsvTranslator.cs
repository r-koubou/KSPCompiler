using System.Collections.Generic;
using System.Text.RegularExpressions;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Commons;
using KSPCompiler.Infrastructures.Commons.Extensions;

using DataTypeUtility = KSPCompiler.Domain.Symbols.MetaData.DataTypeUtility;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Commands.Translators;

internal class FromTsvTranslator : IDataTranslator<string, IReadOnlyCollection<CommandSymbol>>
{
    private enum Column
    {
        Name,
        Reserved,
        Description,
        ReturnType,
        ArgumentBegin
    }

    private static readonly Regex LineComment = new( @"^#.*" );

    public IReadOnlyCollection<CommandSymbol> Translate( string source )
    {
        var result = new List<CommandSymbol>();

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

            TsvUtility.RemoveQuoteCharacter( values );

            var command = new CommandSymbol
            {
                Name        = values[ (int)Column.Name ],
                Reserved    = TsvUtility.ParseBoolean( values[ (int)Column.Reserved ] ),
                Description = values[ (int)Column.Description ],
                DataType    = DataTypeUtility.Guess( values[ (int)Column.ReturnType ] )
            };

            if( values.Length > (int)Column.ArgumentBegin )
            {
                ParseArguments( values, command );
            }

            result.Add( command );
        }

        return result;
    }

    private static void ParseArguments( string[] values, CommandSymbol command )
    {
        /*
         * Argument1, Argument1Description, Argument2, Argument2Description, ...
         */
        TsvUtility.ParseColumnGroups( values, (int)Column.ArgumentBegin, 2, arg =>
            {
                var argument = new CommandArgument
                {
                    Name        = arg[ 0 ],
                    Description = arg[ 1 ],
                    Reserved    = false
                };

                argument.DataType = DataTypeUtility.Guess( argument.Name );
                command.AddArgument( argument );
            }
        );
    }
}
