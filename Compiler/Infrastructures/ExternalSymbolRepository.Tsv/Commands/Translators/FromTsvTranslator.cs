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

    public IReadOnlyCollection<CommandSymbol> Translate( string source )
    {
        var result = new List<CommandSymbol>();

        TsvUtility.ParseTsv( source.SplitNewLine(), TsvUtility.RegexDefaultLineComment, values =>
            {
                TsvUtility.RemoveQuoteCharacter( values );

                var command = new CommandSymbol
                {
                    Name        = values[ (int)Column.Name ],
                    Reserved    = TsvUtility.ParseBoolean( values[ (int)Column.Reserved ] ),
                    Description = values[ (int)Column.Description ],
                    DataType    = DataTypeUtility.GuessFromTypeString( values[ (int)Column.ReturnType ] )
                };

                if( values.Length > (int)Column.ArgumentBegin )
                {
                    ParseArguments( values, command );
                }

                result.Add( command );
            }
        );

        return result;
    }

    private static void ParseArguments( string[] values, CommandSymbol command )
    {
        /*
         * Argument1, Argument1Description, Argument2, Argument2Description, ...
         */
        TsvUtility.ParseColumnGroups( values, (int)Column.ArgumentBegin, 2, arg =>
            {
                var argument = new CommandArgumentSymbol
                {
                    Name        = arg[ 0 ],
                    Description = arg[ 1 ],
                    Reserved    = false
                };

                argument.DataType = DataTypeUtility.GuessFromSymbolName( argument.Name );
                command.AddArgument( argument );
            }
        );
    }
}
