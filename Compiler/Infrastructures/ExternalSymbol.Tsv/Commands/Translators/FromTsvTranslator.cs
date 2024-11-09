using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Tsv.Commons;
using KSPCompiler.Infrastructures.Commons.Extensions;

using DataTypeUtility = KSPCompiler.Domain.Symbols.MetaData.DataTypeUtility;

namespace KSPCompiler.ExternalSymbol.Tsv.Commands.Translators;

internal class FromTsvTranslator : IDataTranslator<string, IReadOnlyCollection<CommandSymbol>>
{
    private enum Column
    {
        Name,
        Reserved,
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
                    Reserved         = TsvUtility.ParseBoolean( values[ (int)Column.Reserved ] ),
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

                // プリミティブ型を表現する文字列が見つからなかった場合はUI型とみなしてリストに追加する
                if( !DataTypeUtility.TryGuessFromTypeString( arg[ 0 ], out var typeFlag ) )
                {
                    typeFlag = DataTypeFlag.None;

                    DataTypeUtility.GuessFromOtherTypeString(
                        arg[ 0 ],
                        out var resultUiTypes,
                        out var resultOtherTypes
                    );

                    uiTypeNames.AddRange( resultUiTypes );
                    otherTypeNames.AddRange( resultOtherTypes );
                }

                var argument = new CommandArgumentSymbol( uiTypeNames, otherTypeNames )
                {
                    Name        = arg[ 1 ],
                    Description = arg[ 2 ],
                    Reserved    = false,
                    DataType    = typeFlag
                };

                command.AddArgument( argument );
            }
        );
    }
}
