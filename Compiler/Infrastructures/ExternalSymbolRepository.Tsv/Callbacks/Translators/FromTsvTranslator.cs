using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Tsv.Commons;
using KSPCompiler.Infrastructures.Commons.Extensions;

using DataTypeUtility = KSPCompiler.Domain.Symbols.MetaData.DataTypeUtility;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Callbacks.Translators;

internal class FromTsvTranslator : IDataTranslator<string, IReadOnlyCollection<CallbackSymbol>>
{
    private enum Column
    {
        Name,
        Reserved,
        Description,
        AllowDuplicate,
        ArgumentBegin
    }

    public IReadOnlyCollection<CallbackSymbol> Translate( string source )
    {
        var result = new List<CallbackSymbol>();

        TsvUtility.ParseTsv( source.SplitNewLine(), TsvUtility.RegexDefaultLineComment, values =>
            {
                TsvUtility.RemoveQuoteCharacter( values );

                var symbol = new CallbackSymbol( TsvUtility.ParseBoolean( values[ (int)Column.AllowDuplicate ] ) )
                {
                    Name        = values[ (int)Column.Name ],
                    Reserved    = TsvUtility.ParseBoolean( values[ (int)Column.Reserved ] ),
                    Description = values[ (int)Column.Description ],
                    DataType    = DataTypeFlag.None
                };

                ParseArguments( values, symbol );
                result.Add( symbol );
            }
        );

        return result;
    }

    private static void ParseArguments( string[] values, CallbackSymbol symbol )
    {
        /*
         * [0] Name
         * [1] Required declare in on_init
         * [2] Description
         */
        TsvUtility.ParseColumnGroups( values, (int)Column.ArgumentBegin, 2, arg =>
            {
                var argument = new CallbackArgumentSymbol( TsvUtility.ParseBoolean( arg[1] ))
                {
                    Name        = arg[ 0 ],
                    Description = arg[ 2 ],
                    Reserved    = false
                };

                argument.DataType = DataTypeUtility.Guess( argument.Name );
                symbol.AddArgument( argument );
            }
        );
    }
}
