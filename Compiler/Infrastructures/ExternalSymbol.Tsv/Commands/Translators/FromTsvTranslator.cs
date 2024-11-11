using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Tsv.Commons;
using KSPCompiler.Infrastructures.Commons.Extensions;

using DataTypeUtility = KSPCompiler.Domain.Symbols.MetaData.DataTypeUtility;

namespace KSPCompiler.ExternalSymbol.Tsv.Commands.Translators;

internal class FromTsvTranslator : IDataTranslator<string, IReadOnlyCollection<CommandSymbol>>
{
    private enum Column
    {
        Name,
        BuiltIn,
        Description,
        BuiltIntoVersion,
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
                    Name             = values[ (int)Column.Name ],
                    BuiltIn          = TsvUtility.ParseBoolean( values[ (int)Column.BuiltIn ] ),
                    Description      = values[ (int)Column.Description ],
                    BuiltIntoVersion = values[ (int)Column.BuiltIntoVersion ],
                    DataType         = DataTypeUtility.GuessFromTypeString( values[ (int)Column.ReturnType ] )
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
         * [0]; type
         * [1]: name
         * [2]: description
         */
        TsvUtility.ParseColumnGroups( values, (int)Column.ArgumentBegin, 3, arg =>
            {
                var uiTypeNames = new List<string>();
                var otherTypeNames = new List<string>();

                DataTypeUtility.GuessFromTypeString( arg[ 0 ], out var typeFlag, ref uiTypeNames, ref otherTypeNames );

                var argument = new CommandArgumentSymbol( uiTypeNames, otherTypeNames )
                {
                    Name        = arg[ 1 ],
                    Description = arg[ 2 ],
                    BuiltIn     = false,
                    DataType    = typeFlag
                };

                command.AddArgument( argument );
            }
        );
    }
}
